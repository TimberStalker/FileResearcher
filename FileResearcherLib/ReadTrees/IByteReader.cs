using FileResearcherLib.DataTypes;
using FileResearcherLib.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileResearcherLib.ReadTrees;
public interface IByteReader
{
    IDataValue ReadBytes(ByteWindow window);
}
