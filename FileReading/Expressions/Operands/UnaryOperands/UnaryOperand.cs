using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileReading.Expressions.Operands.UnaryOperands;
public abstract class UnaryOperand : Operand
{
    public Operand Operand { get; }
    public UnaryOperand(Operand operand)
    {
        Operand = operand;
    }
}
