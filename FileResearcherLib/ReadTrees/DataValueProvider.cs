using FileResearcherLib.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileResearcherLib.ReadTrees;
public class DataValueProvider : IDataValueProvider<ReadTreeNode>
{
    Dictionary<ReadTreeNode, IValueContainer> nodeValues = new Dictionary<ReadTreeNode, IValueContainer>();
    public Func<IDataValue> GetValueProvider(ReadTreeNode node)
    {
        return () =>
        {
            if (nodeValues.TryGetValue(node, out var value))
            {
                return ((ValueContainer)value).Value;
            }
            return new NullDataValue();
        };
    }

    public void Add(ReadTreeNode node, IDataValue value)
    {
        if(node.Reader is DataTypeReader typeReader)
        {
            nodeValues[node] = new ValueContainer(value);
        }
    }
    public void Clear()
    {
        nodeValues.Clear();
    }

    interface IValueContainer
    {

    }
    class ValueContainer : IValueContainer
    {
        public IDataValue Value { get; }

        public ValueContainer(IDataValue value)
        {
            Value = value;
        }
    }
    class ArrayValueContainer : IValueContainer
    {
        public IList<IDataValue> Values { get; } = new List<IDataValue>();
        public ArrayValueContainer()
        {

        }
        public ArrayValueContainer(IEnumerable<IDataValue> values)
        {
            Values = values.ToList();
        }
    }
}
