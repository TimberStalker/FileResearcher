using FileResearcherLib.Expressions;
using FileResearcherLib.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileResearcherLib.DataTypes;
public class AtomicDataType<T> : IDataType
{
    public string Name { get; }
    public IList<ReadParameter> ReadParameters { get; } = new List<ReadParameter>();
    required public Func<AtomicDataType<T>, ByteWindow, IReadOnlyDictionary<ReadParameter, IExpression>, T> ReadBytesFunctor { get; init; }
    public AtomicDataType(string name)
    {
        Name = name;
    }
    [SetsRequiredMembers]
    public AtomicDataType(string name, Func<ByteWindow, IReadOnlyDictionary<ReadParameter, IExpression>, T> readBytesFunctor)
    {
        Name = name;
        ReadBytesFunctor = (type, window, functor) => readBytesFunctor(window, functor);
    }
    public AtomicDataValue<T> CreateValue(T source)
    {
        return new AtomicDataValue<T>(this, source);
    }
    public IReadOnlyList<IReadParameter> GetReadParameters() => ReadParameters.AsEnumerable<IReadParameter>().ToList();

    public AtomicDataValue<T> ReadBytes(ByteWindow window, IReadOnlyDictionary<ReadParameter, IExpression> parameters)
    {
        return CreateValue(ReadBytesFunctor(this, window, parameters));
    }

    IDataValue IDataType.ReadBytes(ByteWindow window, IReadOnlyDictionary<ReadParameter, IExpression> parameters) 
        => ReadBytes(window, parameters);
}
