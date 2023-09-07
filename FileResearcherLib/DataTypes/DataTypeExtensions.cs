using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileResearcherLib.DataTypes;
public static class DataTypeExtensions
{
    public static ReadParameter GetParameter(this IDataType datatype, string name)
    {
        var names = name.Split('.');
        IReadParameter readParameter = datatype.GetReadParameters().First(p => p.Name == names[0]);
        for(int i = 1; i < names.Length; i++)
        {
            if(readParameter is not ReadParameterGroup group)
            {
                throw new Exception();
            }
            readParameter = group.ReadParameters.First(p => p.Name == names[i]);
        }
        if (readParameter is not ReadParameter p) throw new Exception();
        return p;
    }
}
