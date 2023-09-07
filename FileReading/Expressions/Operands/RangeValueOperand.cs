using FileReading.ReadingData.Types.Primitives;
using FileReading.ReadingData.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileReading.Expressions.Operands;
public class RangeValueOperand : Operand
{
	public Operand Start { get; }
	public Operand End { get; }

	public RangeValueOperand(Operand start, Operand end)
	{
		Start = start;
		End = end;
	}

	public override DataValue Evaluate()
	{
		var value1 = Start.Evaluate();
		var value2 = End.Evaluate();

		var idt = IntDataType.Instance;

        if (value1.ConvertableTo(idt))
		{
			value1 = idt.Convert(value1);

        }
        if (value2.ConvertableTo(idt))
        {
            value2 = idt.Convert(value2);

        }

		return new PrimitiveDataValue<Range>(RangeDataType.Instance, ((PrimitiveDataValue<int>)value1).Value..((PrimitiveDataValue<int>)value2).Value);
    }
}
