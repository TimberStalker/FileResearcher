using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileReading.Utils;
using FileReading.Values;

namespace FileReading.Trees;

public abstract class ReadTreeNode
{

    public abstract DataValue Read(ByteStream byteStream);
}
