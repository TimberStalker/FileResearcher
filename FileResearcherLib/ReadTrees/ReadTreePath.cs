using FileResearcherLib.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileResearcherLib.ReadTrees;
public class ReadTreePath : IPathTransformer
{
    public IReadOnlyList<IPathTransformer> Transformers { get; }
    public ReadTreePath(IEnumerable<IPathTransformer> transformers)
    {
        Transformers = transformers.ToArray();
    }
    public IDataValue Transform(IDataValue value)
    {
        foreach (var item in Transformers)
        {
            value = item.Transform(value);
        }
        return value;
    }
    public static ReadTreePath CreateRelativePath(ReadTreeNode from, ReadTreeNode to)
    {
        throw new NotImplementedException();
    }
    public static ReadTreePath CreatePath(ReadTreeNode target)
    {
        List<IPathTransformer> transformers = new List<IPathTransformer>();
        while(target.Parent != null)
        {
            transformers.Insert(0, new ChildAccesserTransformer(target.Parent, target));
            target = target.Parent;
        }
        return new ReadTreePath(transformers);
    }
}
public interface IPathTransformer
{
    IDataValue Transform(IDataValue dataValue);
}
public class ChildAccesserTransformer : IPathTransformer
{
    int index = 0;
    public ChildAccesserTransformer(ReadTreeNode parent, ReadTreeNode child)
    {
        index = parent.ChildNodes.IndexOf(child);
    }
    public IDataValue Transform(IDataValue dataValue)
    {
        if (dataValue is not TreeNodeDataValue treeValue) throw new Exception();
        return treeValue.Children[index];
    }
}