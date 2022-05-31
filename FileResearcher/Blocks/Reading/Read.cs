using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileReading.Blocks.Types;
using FileReading.Blocks.Values;
using FileReading.Utils;
using Name = FileReading.Utils.SharedResource<string>;

namespace FileReading.Blocks.Reading;

public abstract class Read
{
    public DynamicDictionary<string, DataType> parameters;
    public abstract DataValue ReadType(ByteStream byteStream, Dictionary<Name, DataValue>? parameters);

    public Read()
    {
        parameters = new DynamicDictionary<string, DataType>();
    }
    public Read(DynamicDictionary<string, DataType> parameters)
    {
        this.parameters = parameters;
    }
}
