using FileReading.ReadingData.Types.Parameters;
using FileReading.ReadingData.Values;
using FileReading.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileReading.ReadingData.Types.Primitives;

public class IntDataType : NumericPrimitiveDataType
{
    public override string Name => "Int";
    public override Type BackingType => typeof(int);
    public override DataValue Zero => new PrimitiveDataValue<int>(this, 0);
    public override Color Color => Color.FromArgb(0x2c, 0x57, 0xb9);
    public IntDataType() : base(new Guid("901bd372-30fd-4ae3-9eb6-c1c5d7741ba0")) { }

    public override DataValue Read(ByteWindow window, IReadOnlyDictionary<Parameter, DataValue> parameters)
    {
        var read = window.ReadBytes(4, BitConverter.ToInt32);
        return new PrimitiveDataValue<int>(this, read);
    }

    public override bool ConvertableFrom(DataType type) => type is ShortDataType or ByteDataType;
    public override bool CastableFrom(DataType type) => type is DoubleDataType or FloatDataType or LongDataType;

    public override DataValue Convert(DataValue value) => value switch
    {
        PrimitiveDataValue<short> s => new PrimitiveDataValue<int>(this, s.Value),
        PrimitiveDataValue<byte> b => new PrimitiveDataValue<int>(this, b.Value),
        _ => throw new Exception()
    };
    public override DataValue Cast(DataValue value) => value switch
    {
        PrimitiveDataValue<double> d => new PrimitiveDataValue<int>(this, (int)d.Value),
        PrimitiveDataValue<float> f => new PrimitiveDataValue<int>(this, (int)f.Value),
        PrimitiveDataValue<long> l => new PrimitiveDataValue<int>(this, (int)l.Value),
        _ => throw new Exception()
    };
    public override DataValue Add(DataValue value1, DataValue value2) => new PrimitiveDataValue<int>(this, ((PrimitiveDataValue<int>)value1).Value + ((PrimitiveDataValue<int>)value2).Value);
    public override DataValue Subtract(DataValue value1, DataValue value2) => new PrimitiveDataValue<int>(this, ((PrimitiveDataValue<int>)value1).Value - ((PrimitiveDataValue<int>)value2).Value);
    public override DataValue Multiply(DataValue value1, DataValue value2) => new PrimitiveDataValue<int>(this, ((PrimitiveDataValue<int>)value1).Value * ((PrimitiveDataValue<int>)value2).Value);
    public override DataValue Divide(DataValue value1, DataValue value2) => new PrimitiveDataValue<int>(this, ((PrimitiveDataValue<int>)value1).Value / ((PrimitiveDataValue<int>)value2).Value);

    public override bool TryParse(string input, [NotNullWhen(true)] out DataValue? result)
    {
        if(int.TryParse(input, out var value))
        {
            result = new PrimitiveDataValue<int>(this, value);
            return true;
        }
        result = null;
        return false;
    }
    public static IntDataType Instance { get; } = new();
}
