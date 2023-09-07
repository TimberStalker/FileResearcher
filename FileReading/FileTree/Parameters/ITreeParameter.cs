using FileReading.FileTree.Access;
using FileReading.ReadingData.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileReading.FileTree.Parameters;
public interface ITreeParameter
{
    DataValue GetValue(AccessStack accessStack);
    IReadOnlyList<ITreeParameter> ReferenceParameters { get; }
}
