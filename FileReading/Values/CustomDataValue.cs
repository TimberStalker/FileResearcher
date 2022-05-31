using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileReading.Types;
using FileReading.Utils;

namespace FileReading.Values;

public class CustomDataValue : TypeBackedDataValue
{
    Dictionary<SharedResource<string>, DataValue> values;
    public CustomDataValue(DataType baseType) : base(baseType)
    {
        values = new ();
    }

    public override DataValue Get(SharedResource<string> key)
    {
        return values[key];
    }

    public override T Get<T>(string key)
    {
        if (typeof(T) != typeof(DataValue) || !typeof(T).IsSubclassOf(typeof(DataValue)) || typeof(T) != typeof(object)) throw new Exception();
        return (T)(object)values.Single(kv => kv.Key.Value == key).Value;
    }
}
