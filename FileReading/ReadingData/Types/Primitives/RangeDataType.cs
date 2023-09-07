using FileReading.ReadingData.Types.Parameters;
using FileReading.ReadingData.Values;
using FileReading.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FileReading.ReadingData.Types.Primitives;

public class RangeDataType : PrimitiveDataType
{
    public override string Name => "Range";
    public override Type BackingType => typeof(Range);
    public override DataValue Zero => new PrimitiveDataValue<Range>(this, 0..0);
    public override Color Color => Color.FromArgb(0x46, 0xba, 0x2d);
    public RangeDataType() : base(new Guid("f7d7088a-7a47-40c1-a73c-15da3286ba9d")) { }

    public override DataValue Read(ByteWindow window, IReadOnlyDictionary<Parameter, DataValue> parameters)
    {
        var read1 = window.ReadBytes(4, BitConverter.ToInt32);
        var read2 = window.ReadBytes(4, BitConverter.ToInt32);
        return new PrimitiveDataValue<Range>(this, read1.value..read2.value);
    }

    public override bool TryParse(string input, [NotNullWhen(true)] out DataValue? result)
    {
        var match = Regex.Match(input, @"(?'start'\d+)\.\.(?'end'\d+)");
        if(match.Success)
        {
            result = new PrimitiveDataValue<Range>(this, int.Parse(match.Groups["start"].Value)..int.Parse(match.Groups["end"].Value));
            return true;
        }
        result = null;
        return false;
    }
    public static RangeDataType Instance { get; } = new();
}
