using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileReading.Blocks.Types;

namespace FileReading.Blocks.Values;

public class PrimitiveDataValue : DataValue
{
    object value;
    public object Value => value;

    public PrimitiveDataValue(DataType baseType, object value) : base(baseType)
    {
        this.value = value;
    }
}
