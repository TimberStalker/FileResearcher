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

public class LongDataType : NumericPrimitiveDataType
{
    public override string Name => "Long";
    public override Type BackingType => typeof(long);
    public override DataValue Zero => new PrimitiveDataValue<long>(this, 0);
    public override Color Color => Color.FromArgb(0x1c, 0x39, 0x7b);
    public LongDataType() : base(new Guid("339ab0ff-421d-49cd-be6c-ad5b5c96bbf8")) { }

    public override DataValue Read(ByteWindow window, IReadOnlyDictionary<Parameter, DataValue> parameters)
    {
        var read = window.ReadBytes(8, BitConverter.ToInt64);
        return new PrimitiveDataValue<long>(this, read);
    }

    public override bool ConvertableFrom(DataType type) => type is IntDataType or ShortDataType or ByteDataType;
    public override bool CastableFrom(DataType type) => type is DoubleDataType or FloatDataType;

    public override DataValue Convert(DataValue value) => value switch
    {
        PrimitiveDataValue<int> i => new PrimitiveDataValue<long>(this, i.Value),
        PrimitiveDataValue<short> s => new PrimitiveDataValue<long>(this, s.Value),
        PrimitiveDataValue<byte> b => new PrimitiveDataValue<long>(this, b.Value),
        _ => throw new Exception()
    };
    public override DataValue Cast(DataValue value) => value switch
    {
        PrimitiveDataValue<double> d => new PrimitiveDataValue<long>(this, (long)d.Value),
        PrimitiveDataValue<float> f => new PrimitiveDataValue<float>(this, (float)f.Value),
        _ => throw new Exception()
    };
    public override DataValue Add(DataValue value1, DataValue value2) => new PrimitiveDataValue<long>(this, ((PrimitiveDataValue<long>)value1).Value + ((PrimitiveDataValue<long>)value2).Value);
    public override DataValue Subtract(DataValue value1, DataValue value2) => new PrimitiveDataValue<long>(this, ((PrimitiveDataValue<long>)value1).Value - ((PrimitiveDataValue<long>)value2).Value);
    public override DataValue Multiply(DataValue value1, DataValue value2) => new PrimitiveDataValue<long>(this, ((PrimitiveDataValue<long>)value1).Value * ((PrimitiveDataValue<long>)value2).Value);
    public override DataValue Divide(DataValue value1, DataValue value2) => new PrimitiveDataValue<long>(this, ((PrimitiveDataValue<long>)value1).Value / ((PrimitiveDataValue<long>)value2).Value);

    public override bool TryParse(string input, [NotNullWhen(true)] out DataValue? result)
    {
        if(long.TryParse(input, out var value))
        {
            result = new PrimitiveDataValue<long>(this, value);
            return true;
        }
        result = null;
        return false;
    }
    public static LongDataType Instance { get; } = new();
}
