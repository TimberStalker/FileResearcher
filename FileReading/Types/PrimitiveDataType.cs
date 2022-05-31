using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileReading.Values;

namespace FileReading.Types;

public class PrimitiveDataType<T> : DataType
{
    public override PrimitiveDataValue CreateValue()
    {
        throw new NotImplementedException();
    }
}
