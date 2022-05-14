using FileResearcher.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileResearcher.Blocks.Types;

public class DataValue
{
    public DataType DataType { get; private set; }


    public Dictionary<SharedResource<string>, DataValue>? values;


    public DataValue(DataType type)
    {
        this.DataType = type;
    }

    internal void Add(SharedResource<string> name, DataValue value)
    {
        values?.Add(name, value);
    }

    public DataValue? GetValue(SharedResource<string> name)
    {
        if(values is not null)
        {
            if(values.TryGetValue(name, out var value))
            {
                return value;
            }
        }
        return null;
    }

    public DataValue? GetValue(DataPath path)
    {
        return path.GetFrom(this);
    }

    internal void Set(DataPath path, DataValue result)
    {
        var replace = path.GetFrom(this);
        if(replace is not null)
        {
            
        }
    }
}
