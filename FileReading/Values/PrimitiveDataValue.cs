using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileReading.Types;
using FileReading.Utils;

namespace FileReading.Values;

public class PrimitiveDataValue<T> : TypeBackedDataValue
{
    T value;
    public T Value => value;

    public PrimitiveDataValue(DataType baseType) : base(baseType)
    {
    }

    public override T Get<T>(string key)
    {
        if (key == "value")
        {
            if (!Value.GetType().IsSubclassOf(typeof(T))) throw new ArgumentException();
            return (T)Value;
        }
        throw new ArgumentException(key, nameof(key));
    }

    public override DataValue Get(SharedResource<string> key)
    {
        throw new ArgumentException(key.Value, nameof(key));
    }
}
