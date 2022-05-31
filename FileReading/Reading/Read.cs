using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileReading.Types;
using FileReading.Values;
using FileReading.Utils;
using Name = FileReading.Utils.SharedResource<string>;

namespace FileReading.Reading;

public abstract class Read
{
    readonly DataType baseType;
    public DynamicDictionary<string, DataType> parameters;
    public abstract DataValue ReadType(ByteStream byteStream, Dictionary<Name, DataValue> parameters);

    public Read(DataType baseType) : this(baseType, new ())
    {
    }
    public Read(DataType baseType, DynamicDictionary<string, DataType> parameters)
    {
        this.baseType = baseType;
        this.parameters = parameters;
    }
}
