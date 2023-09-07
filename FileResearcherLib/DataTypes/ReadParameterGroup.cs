using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileResearcherLib.DataTypes;
public class ReadParameterGroup : IReadParameter
{
    public string Name { get; set; }
    public IList<IReadParameter> ReadParameters { get; } = new List<IReadParameter>();
    public ReadParameterGroup(string name)
    {
        Name = name;
    }
    public ReadParameterGroup(string name, IEnumerable<IReadParameter> parameters)
    {
        Name = name;
        ReadParameters = parameters.ToList();
    }
}
