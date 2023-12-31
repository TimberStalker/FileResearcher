﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileReading.Blocks.Reading;
using FileReading.Blocks.Values;
using FileReading.Utils;

namespace FileReading.Blocks.Types;

public class DataTypes
{
    public DynamicDictionary<string, DataType> GetPrimitiveTypes()
    {
        DynamicDictionary<string, DataType> types = new();
        var intType = new DataType();
        types.TryAdd("int", intType);
        var intRead = new PrimitiveRead((stream, _) => new PrimitiveDataValue(intType, BitConverter.ToInt32(stream.Read(4))));


        var stringType = new DataType();
        types.TryAdd("string", stringType);
        var stringRead = new PrimitiveRead((stream, _) =>
        {
            int length = BitConverter.ToInt32(stream.Read(4));
            return new PrimitiveDataValue(stringType, BitConverter.ToString(stream.Read(length).ToArray())); 
        });
        DynamicDictionary<string, DataType> stringReadParams = new (("length", intType));
        var stringReadWithParams = new PrimitiveRead(stringReadParams, (stream, param) =>
        {
            int length = (int)(param![stringReadParams.GetKey("length")] as PrimitiveDataValue)!.Value;
            return new PrimitiveDataValue(stringType, BitConverter.ToString(stream.Read(length).ToArray()));
        });

        return types;
    }
}
