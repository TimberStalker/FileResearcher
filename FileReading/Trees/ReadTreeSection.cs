using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileReading.Utils;
using FileReading.Values;

namespace FileReading.Trees
{
    public class ReadTreeSection : ReadTreeNode
    {
        public DynamicDictionary<string, ReadTreeNode> children;

        public ReadTreeSection()
        {
            this.children = new();
        }

        public override DataValue Read(ByteStream byteStream)
        {
            var value = new SectionDataValue();
            foreach (var (name, node) in children)
            {
                var basevalue = node.Read(byteStream);
                value.values.Add(name, basevalue);
            }
            return value;
        }
    }
}
