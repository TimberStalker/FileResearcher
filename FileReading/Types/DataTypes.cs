using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileReading.Reading;
using FileReading.Values;
using FileReading.Utils;

namespace FileReading.Types;

public class DataTypes
{
    public DynamicDictionary<string, DataType> GetPrimitiveTypes()
    {
        DynamicDictionary<string, DataType> types = new();
        var intType = new DataType();
        types.Add("int", intType);
        var intRead = new PrimitiveRead(intType, (stream, _) => new PrimitiveDataValue(intType, BitConverter.ToInt32(stream.Read(4))));


        var stringType = new DataType();
        types.Add("string", stringType);
        var stringRead = new PrimitiveRead(stringType, (stream, _) =>
        {
            int length = BitConverter.ToInt32(stream.Read(4));
            return new PrimitiveDataValue(stringType, BitConverter.ToString(stream.Read(length).ToArray())); 
        });
        DynamicDictionary<string, DataType> stringReadParams = new (("length", intType));
        var stringReadWithParams = new PrimitiveRead(stringType, stringReadParams, (stream, param) =>
        {
            int length = param[stringReadParams.GetKey("length")].Get<int>("value");
            return new PrimitiveDataValue(stringType, BitConverter.ToString(stream.Read(length).ToArray()));
        });

        return types;
    }
}
