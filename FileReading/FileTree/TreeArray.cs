using FileReading.ReadingData.Types.Primitives;
using FileReading.ReadingData.Values;
using FileReading.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileReading.FileTree.Access;
using FileReading.ReadingData.Types;
using System.Net.Http.Headers;
using FileReading.FileTree.Routing;
using System.Xml;

namespace FileReading.FileTree
{
    public class TreeArray : TreeDataType
    {
        public TreeReference Index { get; private set; }

        public TreeArray(Guid id, TreeRoot root, ArrayDataType baseType) : base(id, root, baseType)
        {
            Index = new TreeReference(Guid.NewGuid(), Root, IntDataType.Instance)
            {
                Name = "Index",
            };

            ChildRoute = new TreeRoute(this, new[] { Index });
            Index.ChildRoute = new TreeRoute(Index, Children, IntDataType.Instance);
        }
        public override DataValue ReadBytes(DataValue parent, ByteWindow byteWindow, AccessStack accessStack)
        {
            var result = ReadType.Read(byteWindow, CreateParameters(accessStack));
            result.Name = Name;
            result.Metadata.Set("node", this);
            parent.NextChild = result;

            accessStack.PushNode(this, result);

            var length = (result as PrimitiveDataValue<int>)!.Value;
            for(int i = 0; i < length; i++)
            {
                var arrayItem = new PrimitiveDataValue<int>(IntDataType.Instance, i);
                arrayItem.Name = "ArrayIndex";
                arrayItem.Metadata.Set("node", Index);
                result.NextChild = arrayItem;

                accessStack.PushNode(this, arrayItem);

                foreach (var child in Children)
                {
                    child.ReadBytes(arrayItem, byteWindow, accessStack);
                }

                accessStack.PopNode();
            }

            accessStack.PopNode();

            return result;
        }
        public override void ToXml(XmlWriter writer)
        {
            writer.WriteStartElement("Array");
            writer.WriteAttributeString("Name", Name);
            writer.WriteAttributeString("ReadType", ReadType.Name);
            if (Parameters.Count > 0)
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
    }
}
