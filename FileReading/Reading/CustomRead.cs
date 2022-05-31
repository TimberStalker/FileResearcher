using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileReading.Values;
using FileReading.Utils;

namespace FileReading.Reading;

public class CustomRead : Read
{
    public List<Read> reads = new();
    public override DataValue ReadType(ByteStream byteStream, Dictionary<SharedResource<string>, DataValue>? parameters)
    {
        var datavalue = new DataValue();
    }
}
