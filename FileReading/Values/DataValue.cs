using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileReading.Types;
using FileReading.Utils;

namespace FileReading.Values;

public abstract class DataValue
{
    public abstract void Store(DataValue value);
    public abstract T Get<T>(string key);
    public abstract DataValue Get(SharedResource<string> key);
}
