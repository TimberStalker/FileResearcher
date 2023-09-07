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

public class ShortDataType : NumericPrimitiveDataType
{
    public override string Name => "Short";
    public override Type BackingType => typeof(short);
    public override DataValue Zero => new PrimitiveDataValue<short>(this, 0);
    public override Color Color => Color.FromArgb(0x35, 0x67, 0xd9);
    public ShortDataType() : base(new Guid("f07887f5-6996-490c-8a3d-5aa3015ae45b")) { }

    public override DataValue Read(ByteWindow window, IReadOnlyDictionary<Parameter, DataValue> parameters)
    {
        var read = window.ReadBytes(2, BitConverter.ToInt16);
        return new PrimitiveDataValue<short>(this, read);
    }

    public override bool ConvertableFrom(DataType type) => type is ByteDataType;
    public override bool CastableFrom(DataType type) => type is DoubleDataType or FloatDataType or LongDataType or IntDataType;

    public override DataValue Convert(DataValue value) => value switch
    {
        PrimitiveDataValue<byte> b => new PrimitiveDataValue<short>(this, b.Value),
        _ => throw new Exception()
    };
    public override DataValue Cast(DataValue value) => value switch
    {
        PrimitiveDataValue<double> d => new PrimitiveDataValue<short>(this, (short)d.Value),
        PrimitiveDataValue<float> f => new PrimitiveDataValue<short>(this, (short)f.Value),
        PrimitiveDataValue<long> l => new PrimitiveDataValue<short>(this, (short)l.Value),
        PrimitiveDataValue<int> i => new PrimitiveDataValue<short>(this, (short)i.Value),
        _ => throw new Exception()
    };
    public override DataValue Add(DataValue value1, DataValue value2) => new PrimitiveDataValue<int>(IntDataType.Instance, ((PrimitiveDataValue<short>)value1).Value + ((PrimitiveDataValue<short>)value2).Value);
    public override DataValue Subtract(DataValue value1, DataValue value2) => new PrimitiveDataValue<int>(IntDataType.Instance, ((PrimitiveDataValue<short>)value1).Value - ((PrimitiveDataValue<short>)value2).Value);
    public override DataValue Multiply(DataValue value1, DataValue value2) => new PrimitiveDataValue<int>(IntDataType.Instance, ((PrimitiveDataValue<short>)value1).Value * ((PrimitiveDataValue<short>)value2).Value);
    public override DataValue Divide(DataValue value1, DataValue value2) => new PrimitiveDataValue<int>(IntDataType.Instance, ((PrimitiveDataValue<short>)value1).Value / ((PrimitiveDataValue<short>)value2).Value);


    public override bool TryParse(string input, [NotNullWhen(true)] out DataValue? result)
    {
        if(short.TryParse(input, out var value))
        {
            result = new PrimitiveDataValue<short>(this, value);
            return true;
        }
        result = null;
        return false;
    }
    public static ShortDataType Instance { get; } = new();
}
