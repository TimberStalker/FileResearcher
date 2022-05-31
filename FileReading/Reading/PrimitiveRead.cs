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

internal class PrimitiveRead : Read
{
    Func<ByteStream, Dictionary<Name, DataValue>, DataValue> readFunc;
    public PrimitiveRead(DataType baseType, Func<ByteStream, Dictionary<Name, DataValue>, DataValue> readFunc) : base(baseType)
    {
        this.readFunc = readFunc;
    }
    public PrimitiveRead(DataType baseType, DynamicDictionary<string, DataType> parameters, Func<ByteStream, Dictionary<Name, DataValue>, DataValue> readFunc) : base(baseType, parameters)
    {
        this.readFunc = readFunc;
    }
    public override DataValue ReadType(ByteStream byteStream, Dictionary<Name, DataValue> parameters)
    {
        return readFunc(byteStream, parameters);
    }
}
