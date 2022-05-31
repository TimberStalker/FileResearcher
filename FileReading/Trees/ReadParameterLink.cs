using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileReading.Values;

namespace FileReading.Trees;

public class ReadParameterLink : ReadParameterValue
{
    ReadTreeNode node;

    public ReadParameterLink(ReadTreeNode node)
    {
        this.node = node;
    }

    public override DataValue GetValue()
    {
        return base.GetValue();
    }
}
