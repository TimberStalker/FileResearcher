using FileResearcher.Blocks.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileResearcher.Blocks.Read;

internal class CustomRead : Read
{
    public List<Read>? ReadOperations { get; set; }
    
    public CustomRead(DataType dataType) : base(dataType)
    {
    }

    public CustomRead(DataType dataType, string target) : base(dataType, target)
    {
    }

    public CustomRead(DataType dataType, string target, List<Read> readOperations) : this(dataType, target)
    {
        ReadOperations = readOperations;
    }

    public override DataValue ReadValue(ByteWindow window)
    {
        DataValue value = DataType.CreateValue();
        if (ReadOperations is not null)
        {
            foreach (var read in ReadOperations)
            {
                DataValue result = read.ReadValue(window);
                if (read.Target is not null)
                {
                    value.Set(read.Target, result);
                }
            }
        }

        return value;
    }
}
