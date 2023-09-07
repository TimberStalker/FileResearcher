using FileReading.ReadingData.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileReading.Expressions.Operands;
public class ValueOperand : Operand
{
	DataValue Value { get; }
	public ValueOperand(DataValue value)
	{
		Value = value;
	}
	public override DataValue Evaluate() => Value;
}
