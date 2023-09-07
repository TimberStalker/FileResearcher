using FileResearcherLib.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileResearcherLib.Expressions;
public class ConstantExpression : IExpression
{
    public IDataValue Value { get; }

    public ConstantExpression(IDataValue value)
    {
        Value = value;
    }

    public IDataValue Evaluate() => Value;
}
