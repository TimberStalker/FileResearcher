using FileReading.ReadingData.Types;
using FileReading.ReadingData.Values;
using FileReading.FileTree.Parameters;
using FileReading.Utils;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileReading.FileTree.Access;
using FileReading.ReadingData.Types.Parameters;
using FileReading.FileTree.Routing;
using System.Diagnostics;
using System.Xml;

namespace FileReading.FileTree;

[DebuggerDisplay("{Name}[{Children.Count}]({ReadType.Name}) - {Depth}^{Level}")]
public class TreeDataType : TreeNode
{
    public DataType ReadType { get; }
    public IReadOnlyDictionary<Parameter, TreeParameter> Parameters { get; }

    public TreeDataType(Guid id, TreeRoot root, DataType readType) : base(id, root)
    {
        ReadType = readType;
        Parameters = ReadType.Parameters.ToImmutableDictionary(i => i.Value, i => new TreeParameter(this, i.Value));
    }
    
    public IReadOnlyDictionary<Parameter, DataValue> CreateParameters(AccessStack accessStack)
    {
        return Parameters.Select(p =>
        {
            return KeyValuePair.Create(p.Key, p.Value.GetValue(accessStack));
        }).Where(kv => !kv.Value.IsEmpty).ToImmutableDictionary();
    }

    public void SetParameter(Parameter parameter, string value) => Parameters[parameter].StringValue = value;
    public void SetParameter(Parameter parameter, TreeNode node) 
    {
        Parameters[parameter].StringValue = "\x1A";
        Parameters[parameter].ReferenceParameters.Clear();
        Parameters[parameter].ReferenceParameters.Add(new TreeNodeParameter(this, parameter, node));
    }

    public override void ToXml(XmlWriter writer)
    {
        writer.WriteStartElement("Read");
        writer.WriteAttributeString("Name", Name);
        writer.WriteAttributeString("ReadType", ReadType.Name);
        if(Parameters.Count > 0)
        {
            writer.WriteStartElement("Parameters");
            foreach (var (baseParameter, treeParameter) in Parameters)
            {
                writer.WriteStartElement("Parameter");

                writer.WriteStartElement("BaseParameter");
                writer.WriteAttributeString("Name", baseParameter.Name);
                writer.WriteAttributeString("TypeName", baseParameter.DataType.Name);
                writer.WriteEndElement();

                treeParameter.ToXml(writer);

                writer.WriteEndElement();
            }
            writer.WriteEndElement();
        }
        foreach (var child in Children)
        {
            child.ToXml(writer);
        }

        writer.WriteEndElement();
    }

    public override DataValue ReadBytes(DataValue parent, ByteWindow byteWindow, AccessStack accessStack)
    {
        int startRead = byteWindow.Position;
        var result = ReadType.Read(byteWindow, CreateParameters(accessStack));
        result.Name = Name;
        result.Metadata.Set("node", this);
        result.Metadata.Set("range", startRead..byteWindow.Position);

        parent.NextChild = result;
        
        accessStack.PushNode(this, result);

        foreach (var child in Children)
        {
            child.ReadBytes(result, byteWindow, accessStack);
        }

        accessStack.PopNode();

        return result;
    }
}
