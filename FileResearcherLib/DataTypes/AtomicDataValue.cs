using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileResearcherLib.DataTypes;
public class AtomicDataValue<T> : IDataValue
{
    public AtomicDataType<T> DataType { get; }

    public T Value { get; }


    public AtomicDataValue(AtomicDataType<T> dataType, T value)
    {
        DataType = dataType;
        Value = value;
    }
}
