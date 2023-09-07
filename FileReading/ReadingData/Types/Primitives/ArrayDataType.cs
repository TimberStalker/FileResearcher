using FileReading.ReadingData.Types.Parameters;
using FileReading.ReadingData.Values;
using FileReading.Utils;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileReading.ReadingData.Types.Primitives;

public class ArrayDataType : PrimitiveDataType
{
    public override string Name => "Array";
    public override DataValue Zero => new PrimitiveDataValue<int>(this, 0);
    public override Type BackingType => typeof(int);
    public override Color Color => Color.FromArgb(0xd7, 0x32, 0xbe);
    public override IDictionary<Guid, Parameter> Parameters { get; } = new Dictionary<Guid, Parameter>()
    {
        { new Guid("89e1da56-aa3b-435e-9206-6b8ccde52875"), new Parameter("length", IntDataType.Instance) }
    }.ToImmutableDictionary();


    public ArrayDataType() : base(new Guid("b0e79eb7-f35c-4388-b43e-076752866d1d"))
    {
    }

    public override bool IndexableBy(DataType type) => type is IntDataType;
    public override DataValue Index(DataValue value, DataValue indexer) => value.Children.First(c => c.ValueEquals(indexer));

    public override DataValue Read(ByteWindow window, IReadOnlyDictionary<Parameter, DataValue> parameters)
    {
        int length = 0;
        if (parameters.TryGetValue(GetParameter("length"), out var value))
        {
            length = (value as PrimitiveDataValue<int>)!.Value;
        }
        else
        {
            length = window.ReadBytes(4, BitConverter.ToInt32);
        }

        return new PrimitiveDataValue<int>(this, length);
    }

    public override bool TryParse(string input, out DataValue value)
    {
        throw new NotImplementedException();
    }

    public static ArrayDataType Instance { get; } = new();
}
