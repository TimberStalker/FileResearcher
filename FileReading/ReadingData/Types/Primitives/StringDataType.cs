using FileReading.ReadingData.Types.Parameters;
using FileReading.ReadingData.Values;
using FileReading.Utils;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileReading.ReadingData.Types.Primitives;

public class StringDataType : PrimitiveDataType
{
    public override string Name => "String";
    public override Type BackingType => typeof(string);
    public override DataValue Zero => new PrimitiveDataValue<string>(this, "");
    public override Color Color => Color.FromArgb(0xba, 0x85, 0x2d);
    public override IDictionary<Guid, Parameter> Parameters { get; } = new Dictionary<Guid, Parameter>()
    {
        { new Guid("c369e5a4-b12e-4347-9fa8-fb3b683a4231"), new Parameter("length", IntDataType.Instance) },
        { new Guid("c356e5a4-b48e-9163-3fa8-fb3b428a4231"), new Parameter("nullTerminated", BoolDataType.Instance) }
    }.ToImmutableDictionary();
    public StringDataType() : base(new Guid("428e3e06-10f0-420c-a3a3-4f68048aaa67")) { }
    public override bool AddableTo(DataType type) => type is StringDataType or CharDataType;
    public override bool IndexableBy(DataType type) => type is IntDataType or RangeDataType;
    public override DataValue Index(DataValue value, DataValue indexer) => indexer switch
    {
        PrimitiveDataValue<int> i => new PrimitiveDataValue<char>(CharDataType.Instance, ((PrimitiveDataValue<string>)value).Value[i.Value]),
        PrimitiveDataValue<Range> i => new PrimitiveDataValue<string>(this, ((PrimitiveDataValue<string>)value).Value[i.Value]),
        _ => throw new Exception(),
    };


    public override DataValue Read(ByteWindow window, IReadOnlyDictionary<Parameter, DataValue> parameters) 
    {
        if(parameters.TryGetValue(GetParameter("nullTerminated"), out var v) && v is PrimitiveDataValue<bool> b)
        {
            var stringBytes = new List<byte>(20);
            var readByte = window.ReadBytes(1);
            while (readByte.bytes[0] != 0)
            {
                stringBytes.Add(readByte.bytes[0]);
            }
            return new PrimitiveDataValue<string>(this, Encoding.Default.GetString(stringBytes.ToArray()));
        }
        else
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
            var read = window.ReadBytes(length);
            return new PrimitiveDataValue<string>(this, Encoding.Default.GetString(read));
        }
    }

    public override bool TryParse(string input, [NotNullWhen(true)] out DataValue? result)
    {
        result = new PrimitiveDataValue<string>(this, input);
        return true;
    }
    public static StringDataType Instance { get; } = new();
}
