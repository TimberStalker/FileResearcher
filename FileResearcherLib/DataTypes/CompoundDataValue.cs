using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileResearcherLib.DataTypes;
public class CompoundDataValue : IDataValue, IEnumerable<IDataValue>
{
    public CompoundDataType DataType { get; }

    public IReadOnlyList<IDataValue> Fields { get; } = new List<IDataValue>();
    public IDataValue this[int index] => Fields[index];
    public CompoundDataValue(CompoundDataType dataType, IEnumerable<IDataValue> fields)
    {
        DataType = dataType;
        Fields = fields.ToList();
    }

    public IEnumerator<IDataValue> GetEnumerator()
    {
        return Fields.GetEnumerator();
        //return DataType.Fields.Zip(Fields, (f, v) => KeyValuePair.Create(f, v)).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
