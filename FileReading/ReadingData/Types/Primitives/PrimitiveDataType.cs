using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FileReading.ReadingData.Types.Primitives;

public abstract class PrimitiveDataType : DataType
{
    public abstract Type BackingType { get; }
    protected PrimitiveDataType(Guid id) : base(id) { }
    public override bool Equals(object? obj)
    {
        return obj is PrimitiveDataType p && Id == p.Id;
    }
    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public static IEnumerable<DataType> GetTypes() => Assembly.GetAssembly(typeof(PrimitiveDataType))!.GetTypes().Where(t => !t.IsAbstract && t.IsSubclassOf(typeof(PrimitiveDataType))).Select(t => (DataType)Activator.CreateInstance(t)!);
}
