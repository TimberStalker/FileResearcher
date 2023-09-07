using FileReading.ReadingData.Types;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FileReading.ReadingData.Values;
[DebuggerDisplay(@"{ToString()} \{{BaseType.Name}\}")]
public class CustomDataValue : DataValue
{
    static Regex ToStringRegex = new Regex(@"\{(?'path'.+?)(?::(?'format'.+?))?\}");
    public DataValue[] Values { get; }
    public DataValue this[string name] => Values.First(v => v.Name == name);
    public CustomDataValue(CustomDataType baseType, DataValue[] values) : base(baseType)
    {
        Values = values;
    }
    public override bool ValueEquals(DataValue other)
    {
        return Values.Zip(other.Children).All(v => v.First.ValueEquals(v.Second));
    }
    public override DataValue ShallowClone()
    {
        var result = new CustomDataValue((CustomDataType)BaseType, Values.Select(v => v.ShallowClone()).ToArray());
        result.Name = Name;
        result.Parent = Parent;

        result.Children.AddRange(Children);

        foreach(var (key, value) in Metadata)
        {
            result.Metadata.Set(key, value);
        }
        return result;
    }
    public override DataValue DeepClone()
    {
        var result = new CustomDataValue((CustomDataType)BaseType, Values.Select(v => v.DeepClone()).ToArray());
        result.Name = Name;
        result.Parent = Parent;

        result.Children.AddRange(Children.Select(v => v.DeepClone()));

        foreach (var (key, value) in Metadata)
        {
            result.Metadata.Set(key, value);
        }
        return result;
    }
    public override string ToString()
    {
        if (BaseType is CustomDataType c)
        {
            if (c.HasCustomStringFormat)
            {
                return ToStringRegex.Replace(c.CustomStringFormat, match =>
                {
                    var path = match.Groups["path"].Value.Split('.');
                    DataValue currentValue = this;
                    foreach (var item in path)
                    {
                        if (currentValue is CustomDataValue value)
                        {
                            if (int.TryParse(item, out var index))
                            {
                                currentValue = value.Values[index];
                            }
                            else
                            {
                                currentValue = value[item];
                            }
                        }
                        else
                        {
                            throw new Exception("No longer within custom value.");
                        }
                    }
                    if (match.Groups["format"].Success)
                    {
                        return currentValue.ToString(match.Groups["format"].Value);
                    }
                    else
                    {
                        return currentValue.ToString()!;
                    }
                });
            }
        }

        return $"({string.Join<DataValue>(", ", Values)})";
    }
}
