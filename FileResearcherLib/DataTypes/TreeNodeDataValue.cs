using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileResearcherLib.DataTypes;
public class TreeNodeDataValue : IDataValue
{
    public string Name { get; init; } = "";
    public IDataValue? Self { get; set; }
    public IList<TreeNodeDataValue> Children { get; } = new List<TreeNodeDataValue>();
}
