using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileReading.Blocks.Values;
using FileReading.Utils;

namespace FileReading.Blocks.Reading;

public class CustomRead : Read
{
    public override DataValue ReadType(ByteStream byteStream, Dictionary<SharedResource<string>, DataValue>? parameters)
    {
        throw new NotImplementedException();
    }
}
