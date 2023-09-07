using FileResearcherLib.DataTypes;
using FileResearcherLib.ReadTrees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileResearcherLib.Expressions;
public class TreeSourceExpression : IExpression
{
    ReadTreePath TreePath { get; }
    public Func<IDataValue> ValueRootProvider { get; }

    public TreeSourceExpression(ReadTreePath treePath, Func<IDataValue> valueRootProvider)
    {
        TreePath = treePath;
        ValueRootProvider = valueRootProvider;
    }
    public IDataValue Evaluate()
    {
        var root = ValueRootProvider();
        var transformed = TreePath.Transform(root);
        if(transformed is TreeNodeDataValue treeValue)
        {
            transformed = treeValue.Self!;
        }
        return transformed;
    }
}
