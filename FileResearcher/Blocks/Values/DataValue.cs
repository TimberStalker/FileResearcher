using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileReading.Blocks.Types;

namespace FileReading.Blocks.Values;

public class DataValue
{
    DataType baseType;

    public DataType BaseType => baseType;
    public DataValue(DataType baseType)
    {
        this.baseType = baseType;
    }
}
