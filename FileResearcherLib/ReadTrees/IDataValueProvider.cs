using FileResearcherLib.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileResearcherLib.ReadTrees;
public interface IDataValueProvider<TSource>
{
    void Add(TSource source, IDataValue value);
    Func<IDataValue> GetValueProvider(TSource source);
    void Clear();
}
