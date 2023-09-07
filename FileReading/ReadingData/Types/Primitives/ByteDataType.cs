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

public class ByteDataType : NumericPrimitiveDataType
{
    public override string Name => "Byte";
    public override Type BackingType => typeof(byte);
    public override DataValue Zero => new PrimitiveDataValue<byte>(this, 0);
    public override Color Color => Color.FromArgb(0x3b, 0x72, 0xf0);
    public ByteDataType() : base(new Guid("16f3ceda-0f60-4dde-925b-50f78835c8bf")) { }

    public override DataValue Read(ByteWindow window, IReadOnlyDictionary<Parameter, DataValue> parameters)
    {
        var read = window.ReadBytes(1);
        return new PrimitiveDataValue<byte>(this, read.bytes[0]);
    }

    public override bool CastableFrom(DataType type) => type is DoubleDataType or FloatDataType or LongDataType or IntDataType or ShortDataType;

    public override DataValue Cast(DataValue value) => value switch
    {
        PrimitiveDataValue<double> d => new PrimitiveDataValue<byte>(this, (byte)d.Value),
        PrimitiveDataValue<float> f => new PrimitiveDataValue<byte>(this, (byte)f.Value),
        PrimitiveDataValue<long> l => new PrimitiveDataValue<byte>(this, (byte)l.Value),
        PrimitiveDataValue<int> i => new PrimitiveDataValue<byte>(this, (byte)i.Value),
        PrimitiveDataValue<short> s => new PrimitiveDataValue<byte>(this, (byte)s.Value),
        _ => throw new Exception()
    };
    public override DataValue Add(DataValue value1, DataValue value2) => new PrimitiveDataValue<int>(IntDataType.Instance, ((PrimitiveDataValue<byte>)value1).Value + ((PrimitiveDataValue<byte>)value2).Value);
    public override DataValue Subtract(DataValue value1, DataValue value2) => new PrimitiveDataValue<int>(IntDataType.Instance, ((PrimitiveDataValue<byte>)value1).Value - ((PrimitiveDataValue<byte>)value2).Value);
    public override DataValue Multiply(DataValue value1, DataValue value2) => new PrimitiveDataValue<int>(IntDataType.Instance, ((PrimitiveDataValue<byte>)value1).Value * ((PrimitiveDataValue<byte>)value2).Value);
    public override DataValue Divide(DataValue value1, DataValue value2) => new PrimitiveDataValue<int>(IntDataType.Instance, ((PrimitiveDataValue<byte>)value1).Value / ((PrimitiveDataValue<byte>)value2).Value);


    public override bool TryParse(string input, [NotNullWhen(true)] out DataValue? result)
    {
        if(byte.TryParse(input, out var value))
        {
            result = new PrimitiveDataValue<byte>(this, value);
            return true;
        }
        result = null;
        return false;
    }
    public static ByteDataType Instance { get; } = new();
}
