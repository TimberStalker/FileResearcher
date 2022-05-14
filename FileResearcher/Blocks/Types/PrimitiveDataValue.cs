using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileResearcher.Blocks.Types
{
    public class PrimitiveDataValue : DataValue
    {
        public object Value { get; set; }
        public PrimitiveDataValue(object value, DataType type) : base(type)
        {
            Value = value;
        }
    }
}
