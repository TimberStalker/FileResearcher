using FileResearcherLib.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileResearcherLib.DataTypes;
public class ReadParameter : IReadParameter
{
    public string Name { get; } = "";
    public IDataType DataType { get; }
    public Func<IExpression> DefaultValueExpression { get; } = () => new ConstantExpression(null!);

    public ReadParameter(string name, IDataType dataType, Func<IExpression> defaultValueExpression)
    {
        Name = name;
        DataType = dataType;
        DefaultValueExpression = defaultValueExpression;
    }
}
