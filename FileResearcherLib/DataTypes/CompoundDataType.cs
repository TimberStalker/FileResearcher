using FileResearcherLib.Expressions;
using FileResearcherLib.ReadTrees;
using FileResearcherLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileResearcherLib.DataTypes;
public class CompoundDataType : IDataType
{
    public string Name { get; set; } = "";
    public IList<ReadParameter> ReadParameters { get; } = new List<ReadParameter>();
    public ReadTreeNode ReadTree { get; init; } = new ReadTreeNode();
    public CompoundDataType()
    {
        
    }

    public TreeNodeDataValue ReadBytes(ByteWindow window, IReadOnlyDictionary<ReadParameter, IExpression> parameters)
    {
        return ReadTree.ReadBytes(window);
    }

    public IReadOnlyList<IReadParameter> GetReadParameters()
    {
        var result = new List<IReadParameter>(ReadParameters);
        return result;
    }

    IDataValue IDataType.ReadBytes(ByteWindow window, IReadOnlyDictionary<ReadParameter, IExpression> parameters)
        => ReadBytes(window, parameters);
}
