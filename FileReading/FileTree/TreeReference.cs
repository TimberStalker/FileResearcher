using FileReading.FileTree.Access;
using FileReading.FileTree.Parameters;
using FileReading.FileTree.Routing;
using FileReading.ReadingData.Types;
using FileReading.ReadingData.Values;
using FileReading.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace FileReading.FileTree;

public class TreeReference : TreeNode
{
    public TreeParameter Value { get; }
    public TreeReference(Guid id, TreeRoot root, DataType dataType) : base(id, root)
    {
        Value = new TreeParameter(this, new CustomBaseParameter("Reference", dataType));
    }
    public TreeReference(Guid id, TreeRoot root, TreeRoute childRoute, DataType dataType) : base(id, root, childRoute)
    {
        Value = new TreeParameter(this, new CustomBaseParameter("Reference", dataType));
    }

    public override DataValue ReadBytes(DataValue parent, ByteWindow byteWindow, AccessStack accessStack)
    {
        var result = Value.GetValue(accessStack).ShallowClone();
        result.Name = Name;
        result.Metadata.Set("node", this);

        parent.NextChild = result;

        accessStack.PushNode(this, result);

        foreach (var child in Children)
        {
            child.ReadBytes(result, byteWindow, accessStack);
        }

        accessStack.PopNode();

        return result;
    }
    public override void ToXml(XmlWriter writer)
    {
        writer.WriteStartElement("Reference");
        writer.WriteAttributeString("Name", Name);

        writer.WriteStartElement("BaseParameter");
        writer.WriteAttributeString("Name", Value.BaseParameter.Name);
        writer.WriteAttributeString("TypeName", Value.BaseParameter.DataType.Name);
        writer.WriteEndElement();

        Value.ToXml(writer);

        foreach (var child in Children)
        {
            child.ToXml(writer);
        }

        writer.WriteEndElement();
    }
}
