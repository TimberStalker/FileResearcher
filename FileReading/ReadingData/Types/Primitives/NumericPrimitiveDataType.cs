using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileReading.ReadingData.Types.Primitives;
public abstract class NumericPrimitiveDataType : PrimitiveDataType
{
    protected NumericPrimitiveDataType(Guid id) : base(id)
    {
    }

    public override bool AddableTo(DataType type) => type.GetType() == GetType();
    public override bool SubtractibleTo(DataType type) => type.GetType() == GetType();
    public override bool MultipliableTo(DataType type) => type.GetType() == GetType();
    public override bool DivisibleTo(DataType type) => type.GetType() == GetType();
}
