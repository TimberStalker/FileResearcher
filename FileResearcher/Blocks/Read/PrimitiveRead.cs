using FileResearcher.Blocks.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileResearcher.Blocks.Read
{
    internal class PrimitiveRead : Read
    {
        public Func<ByteWindow, object> Func { get; }

        public PrimitiveRead(DataType dataType, Func<ByteWindow,object> func) : base(dataType)
        {
            Func = func;
        }

        public PrimitiveRead(DataType dataType, string target, Func<ByteWindow, object> func) : base(dataType, target)
        {
            Func = func;
        }

        public override DataValue ReadValue(ByteWindow window)
        {
            var value = DataType.CreateValue() as PrimitiveDataValue;

            value!.Value = Func(window);
            
            return value;
        }
    }
}
