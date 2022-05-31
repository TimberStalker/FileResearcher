using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileReading.Types;

namespace FileReading.Values;

public abstract class TypeBackedDataValue : DataValue
{
    DataType baseType;

    public DataType BaseType => baseType;
    public TypeBackedDataValue(DataType baseType)
    {
        this.baseType = baseType;
    }
}
