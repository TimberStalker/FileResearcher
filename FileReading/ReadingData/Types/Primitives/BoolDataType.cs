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

public class BoolDataType : PrimitiveDataType
{
    public override string Name => "Bool";
    public override Type BackingType => typeof(bool);
    public override DataValue Zero => new PrimitiveDataValue<bool>(this, false);
    public override Color Color => Color.FromArgb(0x6a, 0x2d, 0xba);
    public BoolDataType() : base(new Guid("16d2f164-2464-4f37-98b4-ae7cf5ed7402")) { }

    public override DataValue Read(ByteWindow window, IReadOnlyDictionary<Parameter, DataValue> parameters) 
    {
        var read = window.ReadBytes(4, BitConverter.ToBoolean);
        return new PrimitiveDataValue<bool>(this, read);
    } 

    public override bool TryParse(string input, [NotNullWhen(true)] out DataValue? result)
    {
        if(bool.TryParse(input, out var value))
        {
            result = new PrimitiveDataValue<bool>(this, value);
            return true;
        }
        result = null;
        return false;
    }

    public static BoolDataType Instance { get; } = new();
}
