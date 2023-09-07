using FileResearcherLib.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileResearcherLib.Expressions;
public interface IExpression
{
    IDataValue Evaluate();
}
