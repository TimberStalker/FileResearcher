using FileResearcher.Blocks.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileResearcher.Blocks.Read;

public abstract class Read
{
    public DataType DataType { get; set; }
    public string? Target { get; set; }

    public Read(DataType dataType)
    {
        DataType = dataType;
    }
    public Read(DataType dataType, string target)
    {
        DataType = dataType;
        this.Target = target;
    }

    public abstract DataValue ReadValue(ByteWindow window);
}
