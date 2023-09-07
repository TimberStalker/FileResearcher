using FileReading.ReadingData.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileReading.Expressions.Operands.UnaryOperands;
public class CastOperand : UnaryOperand
{
    public CastOperand(Operand operand, string castType) : base(operand)
    {

    }
    public override DataValue Evaluate() => Operand.Evaluate();
}
