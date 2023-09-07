using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileReading.ReadingData.Types.Parameters;

public class Parameter : IParameter
{
    public string Name { get; set; }
    public Guid Id { get; set; }
    public DataType DataType { get; set; }

    public Parameter(string name, DataType dataType) : this(Guid.NewGuid(), name, dataType) { }
    public Parameter(Guid id, string name, DataType dataType)
    {
        Name = name;
        Id = id;
        DataType = dataType;
    }
}
