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

public class CharDataType : PrimitiveDataType
{
    public override string Name => "Char";
    public override Type BackingType => typeof(char);
    public override DataValue Zero => new PrimitiveDataValue<char>(this, (char)0x00);
    public override Color Color => Color.FromArgb(0xf2, 0xa1, 0x3f);
    public CharDataType() : base(new Guid("28dca5b9-f672-4668-a509-91121f6abef1")) { }
    public override bool AddableTo(DataType type) => type is StringDataType or CharDataType;
    public override bool IndexableBy(DataType type) => type is IntDataType or RangeDataType;

    public override DataValue Read(ByteWindow window, IReadOnlyDictionary<Parameter, DataValue> parameters) 
    {
        var read = window.ReadBytes(1);
        return new PrimitiveDataValue<char>(this, (char)read.bytes[0]);
    }

    public override bool TryParse(string input, [NotNullWhen(true)] out DataValue? result)
    {
        result = input.Length != 1 ? null : new PrimitiveDataValue<char>(this, input[0]);
        return result != null;
    }
    public static CharDataType Instance { get; } = new();
}
