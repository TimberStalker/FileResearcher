using FileResearcherLib.DataTypes;
using FileResearcherLib.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileResearcherLib.ReadTrees;
public class ReadTreeNode
{
    public ReadTreeNode Root => Parent is null ? this : Parent.Root;
    public ReadTreeNode? Parent { get; private set; }
    public string Name { get; set; } = "";
    public IList<ReadTreeNode> ChildNodes { get; }

    public IByteReader? Reader { get; set; }


    public IDataValueProvider<ReadTreeNode>? DataValueProvider { get; set; }

    public ReadTreeNode()
    {
        ChildNodes = new ReadTreeChildList(this) { };
    }
    public ReadTreeNode(string name) : this()
    {
        Name = name;
    }
    public ReadTreeNode(out ReadTreeNode node) : this()
    {
        node = this;
        ChildNodes = new ReadTreeChildList(this) { };
    }
    public ReadTreeNode(string name, out ReadTreeNode node) : this(name)
    {
        node = this;
        Name = name;
    }

    public TreeNodeDataValue ReadBytes(ByteWindow window)
    {
        var dataValue = new TreeNodeDataValue()
        {
            Name = Name
        };
        DataValueProvider?.Add(this, dataValue);
        dataValue.Self = Reader?.ReadBytes(window);
        foreach(var node in ChildNodes)
        {
            dataValue.Children.Add(node.ReadBytes(window, dataValue));
        }
        return dataValue;
    }

    public TreeNodeDataValue ReadBytes(ByteWindow window, TreeNodeDataValue top)
    {
        var dataValue = new TreeNodeDataValue()
        {
            Name = Name
        };
        DataValueProvider?.Add(this, dataValue);
        dataValue.Self = Reader?.ReadBytes(window);
        foreach (var node in ChildNodes)
        {
            dataValue.Children.Add(node.ReadBytes(window, top));
        }
        return dataValue;
    }
    class ReadTreeChildList : IList<ReadTreeNode>
    {
        List<ReadTreeNode> ChildNodes { get; } = new List<ReadTreeNode>();
        ReadTreeNode Owner { get; }
        public ReadTreeChildList(ReadTreeNode owner)
        {
            Owner = owner;
        }
        public ReadTreeNode this[int index] { get => ChildNodes[index]; set => ChildNodes[index] = value; }

        public int Count => ChildNodes.Count;

        public bool IsReadOnly => false;

        public void Add(ReadTreeNode item)
        {
            item.Parent = Owner;
            ChildNodes.Add(item);
        }

        public void Clear() => ChildNodes.Clear();

        public bool Contains(ReadTreeNode item) => ChildNodes.Contains(item);

        public void CopyTo(ReadTreeNode[] array, int arrayIndex) => ChildNodes.CopyTo(array, arrayIndex);

        public IEnumerator<ReadTreeNode> GetEnumerator() => ChildNodes.GetEnumerator();

        public int IndexOf(ReadTreeNode item) => ChildNodes.IndexOf(item);

        public void Insert(int index, ReadTreeNode item) => ChildNodes.Insert(index, item);

        public bool Remove(ReadTreeNode item) => ChildNodes.Remove(item);

        public void RemoveAt(int index) => ChildNodes.RemoveAt(index);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
