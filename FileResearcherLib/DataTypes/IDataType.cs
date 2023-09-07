using FileResearcherLib.Expressions;
using FileResearcherLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileResearcherLib.DataTypes;
public interface IDataType
{
    string Name { get; }
    IReadOnlyList<IReadParameter> GetReadParameters();
    IDataValue ReadBytes(ByteWindow window, IReadOnlyDictionary<ReadParameter, IExpression> parameters);
}
