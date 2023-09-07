using FileReading.ReadingData.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileReading.Expressions.Operands.BinaryOperands;
public class MultiplyOperand : BinaryOperand
{
    public MultiplyOperand(Operand operand1, Operand operand2) : base(operand1, operand2)
    {
    }

    public override DataValue Evaluate()
    {
        var value1 = Operand1.Evaluate();
        var value2 = Operand2.Evaluate();

        if(value1.MultipliableTo(value2))
        {
            return value1.Multiply(value2);
        } else if (value2.MultipliableTo(value2) && value1.ConvertableTo(value2.BaseType))
        {
            return value2.BaseType.Convert(value1).Multiply(value2);
        }
        else if (value1.MultipliableTo(value1) && value2.ConvertableTo(value1.BaseType))
        {
            return value1.Multiply(value1.BaseType.Convert(value2));
        }

        throw new Exception();
    }
}
