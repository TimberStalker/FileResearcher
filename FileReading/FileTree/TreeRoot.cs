using FileReading.FileTree.Access;
using FileReading.FileTree.Routing;
using FileReading.ReadingData.Types;
using FileReading.ReadingData.Values;
using FileReading.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace FileReading.FileTree;

[DebuggerDisplay("{Name}[{Children.Count}](Root) - {Depth}^{Level}")]
public class TreeRoot : TreeNode
{
    public TreeRoot(Guid id) : base(id, null!)
    {
        Root = this;
    }

    public override DataValue ReadBytes(DataValue parent, ByteWindow byteWindow, AccessStack accessStack)
    {
        throw new NotImplementedException();
    }
    public IEnumerable<DataValue> ReadBytes(ByteWindow byteWindow)
    {
        var rootValue = DataValue.Empty;
        rootValue.Metadata.Set("node", this);
        var stack = new AccessStack(rootValue);
        stack.PushNode(this, rootValue);
        foreach (var child in Children)
        {
            var childResult = child.ReadBytes(rootValue, byteWindow, stack);
            yield return childResult;
        }
        stack.PopNode();
    }
    public override void ToXml(XmlWriter writer)
    {
        writer.WriteStartElement("Root");
        writer.WriteAttributeString("Name", Name);
        writer.WriteAttributeString("Id", Id.ToString());
        foreach (var child in Children)
        {
            child.ToXml(writer);
        }

        writer.WriteEndElement();
    }
}
