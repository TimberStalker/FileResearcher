using FileResearcherLib.DataTypes;
using FileResearcherLib.Expressions;
using FileResearcherLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileResearcherLib.ReadTrees;
public class DataTypeReader : IByteReader
{
    public IDataType DataType { get; }
    public IDictionary<ReadParameter, IExpression> Parameters { get; } = new Dictionary<ReadParameter, IExpression>();
    public DataTypeReader(IDataType dataType)
    {
        DataType = dataType;
    }

    public IDataValue ReadBytes(ByteWindow window)
    {
        return DataType.ReadBytes(window, (IReadOnlyDictionary<ReadParameter, IExpression>)Parameters);
    }
}
