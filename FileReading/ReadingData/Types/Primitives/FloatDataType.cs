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

public class FloatDataType : NumericPrimitiveDataType
{
    public override string Name => "Float";
    public override Type BackingType => typeof(float);
    public override DataValue Zero => new PrimitiveDataValue<float>(this, 0);
    public override Color Color => Color.FromArgb(0x2e, 0x2d, 0xba);
    public FloatDataType() : base(new Guid("b4fe52bc-8cdb-41ea-b251-5f22f7bbee65")) { }

    public override DataValue Read(ByteWindow window, IReadOnlyDictionary<Parameter, DataValue> parameters)
    {
        var read = window.ReadBytes(4, BitConverter.ToSingle);
        return new PrimitiveDataValue<float>(this, read);
    }

    public override bool ConvertableFrom(DataType type) => type is LongDataType or IntDataType or ShortDataType or ByteDataType;
    public override bool CastableFrom(DataType type) => type is DoubleDataType;

    public override DataValue Convert(DataValue value) => value switch
    {
        PrimitiveDataValue<long> l => new PrimitiveDataValue<float>(this, l.Value),
        PrimitiveDataValue<int> i => new PrimitiveDataValue<float>(this, i.Value),
        PrimitiveDataValue<short> s => new PrimitiveDataValue<float>(this, s.Value),
        PrimitiveDataValue<byte> b => new PrimitiveDataValue<float>(this, b.Value),
        _ => throw new Exception()
    };
    public override DataValue Cast(DataValue value) => value switch
    {
        PrimitiveDataValue<double> d => new PrimitiveDataValue<float>(this, (float)d.Value),
        _ => throw new Exception()
    };

    public override DataValue Add(DataValue value1, DataValue value2) => new PrimitiveDataValue<float>(this, ((PrimitiveDataValue<float>)value1).Value + ((PrimitiveDataValue<float>)value2).Value);
    public override DataValue Subtract(DataValue value1, DataValue value2) => new PrimitiveDataValue<float>(this, ((PrimitiveDataValue<float>)value1).Value - ((PrimitiveDataValue<float>)value2).Value);
    public override DataValue Multiply(DataValue value1, DataValue value2) => new PrimitiveDataValue<float>(this, ((PrimitiveDataValue<float>)value1).Value * ((PrimitiveDataValue<float>)value2).Value);
    public override DataValue Divide(DataValue value1, DataValue value2) => new PrimitiveDataValue<float>(this, ((PrimitiveDataValue<float>)value1).Value / ((PrimitiveDataValue<float>)value2).Value);

    public override bool TryParse(string input, [NotNullWhen(true)] out DataValue? result)
    {
        if(float.TryParse(input, out var value))
        {
            result = new PrimitiveDataValue<float>(this, value);
            return true;
        }
        result = null;
        return false;
    }
    public static FloatDataType Instance { get; } = new();
}
