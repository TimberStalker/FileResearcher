using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileReading.Expressions.Operands.BinaryOperands;
public abstract class BinaryOperand : Operand
{
    public Operand Operand1 { get; }
    public Operand Operand2 { get; }
    public BinaryOperand(Operand operand1, Operand operand2)
    {
        Operand1 = operand1;
        Operand2 = operand2;
    }
}
