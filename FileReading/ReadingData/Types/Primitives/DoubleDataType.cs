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

public class DoubleDataType : NumericPrimitiveDataType
{
    public override string Name => "Double";
    public override Type BackingType => typeof(float);
    public override DataValue Zero => new PrimitiveDataValue<float>(this, 0);
    public override Color Color => Color.FromArgb(0x22, 0x22, 0x90);
    public DoubleDataType() : base(new Guid("306df2e9-5a27-4917-a24b-df943a82eba1")) { }

    public override DataValue Read(ByteWindow window, IReadOnlyDictionary<Parameter, DataValue> parameters)
    {
        var read = window.ReadBytes(4, BitConverter.ToSingle);
        return new PrimitiveDataValue<float>(this, read);
    }
    public override bool ConvertableFrom(DataType type) => type is FloatDataType or LongDataType or IntDataType or ShortDataType or ByteDataType;

    public override DataValue Convert(DataValue value) => value switch
    {
        PrimitiveDataValue<float> f => new PrimitiveDataValue<double>(this, f.Value),
        PrimitiveDataValue<long> l => new PrimitiveDataValue<double>(this, l.Value),
        PrimitiveDataValue<int> i => new PrimitiveDataValue<double>(this, i.Value),
        PrimitiveDataValue<short> s => new PrimitiveDataValue<double>(this, s.Value),
        PrimitiveDataValue<byte> b => new PrimitiveDataValue<double>(this, b.Value),
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
    public static DoubleDataType Instance { get; } = new();
}
