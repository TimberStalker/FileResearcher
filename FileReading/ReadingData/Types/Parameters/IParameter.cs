using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileReading.ReadingData.Types.Parameters;

public interface IParameter
{
    string Name { get; }
    DataType DataType { get; }
}
