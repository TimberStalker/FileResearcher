using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileReading.Reading;
using FileReading.Utils;
using FileReading.Values;

namespace FileReading.Types;

public abstract class DataType
{
    public List<Read> reads;

    public DynamicDictionary<string, DataType> baseTypes;

    public DataType()
    {
        this.reads = new();
        baseTypes = new();
    }

    public abstract DataValue CreateValue();
}
