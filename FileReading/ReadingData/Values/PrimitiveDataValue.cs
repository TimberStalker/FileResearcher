using FileReading.ReadingData.Types;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace FileReading.ReadingData.Values;
[DebuggerDisplay(@"{Value} \{{BaseType.Name}\}")]
public class PrimitiveDataValue<T> : DataValue where T : notnull
{
    public T Value { get; }

    public PrimitiveDataValue(DataType baseType, T value) : base(baseType)
    {
        Value = value;
    }
    public override bool ValueEquals(DataValue other)
    {
        return other is PrimitiveDataValue<T> p && Value.Equals(p.Value);
    }
    public override DataValue ShallowClone()
    {
        var result = new PrimitiveDataValue<T>(BaseType, Value);
        result.Name = Name;
        result.Parent = Parent;

        result.Children.AddRange(Children);

        foreach (var (key, value) in Metadata)
        {
            result.Metadata.Set(key, value);
        }
        return result;
    }
    public override DataValue DeepClone()
    {
        var result = new PrimitiveDataValue<T>((CustomDataType)BaseType, Value);
        result.Name = Name;
        result.Parent = Parent;

        result.Children.AddRange(Children.Select(c => c.DeepClone()));

        foreach (var (key, value) in Metadata)
        {
            result.Metadata.Set(key, value);
        }
        return result;
    }
    public override string ToString() => Value.ToString()!;
    public override string ToString(string format)
    {
        var formatMethod = typeof(T).GetMethod("ToString", new Type[] { typeof(string) });
        if(formatMethod is not null)
        {
            return (string)formatMethod.Invoke(Value, new object[] { format })!;
        }
        return ToString();
    }
}
