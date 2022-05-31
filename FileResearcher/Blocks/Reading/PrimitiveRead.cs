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

internal class PrimitiveRead : Read
{
    Func<ByteStream, Dictionary<Name, DataValue>?, DataValue> readFunc;
    public PrimitiveRead(Func<ByteStream, Dictionary<Name, DataValue>?, DataValue> readFunc) : base()
    {
        this.readFunc = readFunc;
    }
    public PrimitiveRead(DynamicDictionary<string, DataType> parameters, Func<ByteStream, Dictionary<Name, DataValue>?, DataValue> readFunc) : base(parameters)
    {
        this.readFunc = readFunc;
    }
    public override DataValue ReadType(ByteStream byteStream, Dictionary<Name, DataValue>? parameters)
    {
        if (this.parameters.Count == 0 && (parameters is null || parameters.Count == 0))
            return readFunc(byteStream, null);

        if (this.parameters is null || parameters is null) throw new InvalidOperationException();

        if(this.parameters.Count != parameters.Count) throw new InvalidOperationException();

        foreach (var (key, value) in this.parameters)
        {
            if (!parameters.ContainsKey(key)) throw new InvalidOperationException();

            if(parameters[key].BaseType != value) throw new InvalidOperationException();
        }

        return readFunc(byteStream, parameters);
    }
}
