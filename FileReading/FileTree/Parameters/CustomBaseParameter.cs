using FileReading.ReadingData.Types;
using FileReading.ReadingData.Types.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileReading.FileTree.Parameters;

public class CustomBaseParameter : IParameter
{
    public string Name { get; }

    public DataType DataType { get; }
    public CustomBaseParameter(string name, DataType dataType)
    {
        Name = name;
        DataType = dataType;
    }
}
