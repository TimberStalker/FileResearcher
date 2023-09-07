using FileReading.ReadingData.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileReading.Expressions.Operands.BinaryOperands;
public class IndexerOperand : BinaryOperand
{
    public IndexerOperand(Operand operand1, Operand operand2) : base(operand1, operand2)
    {
    }
    public override DataValue Evaluate() => Operand1.Evaluate().Index(Operand2.Evaluate());

}
