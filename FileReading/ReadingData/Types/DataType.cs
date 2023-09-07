using FileReading.ReadingData.Types.Parameters;
using FileReading.ReadingData.Values;
using FileReading.Utils;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace FileReading.ReadingData.Types;

public abstract class DataType
{
    public abstract string Name { get; }
    public Guid Id { get; init; }
    public bool IsEmpty => this is EmptyDataType;
    public virtual IDictionary<Guid, Parameter> Parameters { get; } = ImmutableDictionary<Guid, Parameter>.Empty;
    public abstract DataValue Zero { get; }
    public abstract Color Color { get; }
    public DataType(Guid id)
    {
        Id = id;
    }

    public virtual bool ConvertableFrom(DataType type) => false;
    public virtual bool CastableFrom(DataType type) => false;

    public virtual bool AddableTo(DataType type) => false;
    public virtual bool SubtractibleTo(DataType type) => false;
    public virtual bool MultipliableTo(DataType type) => false;
    public virtual bool DivisibleTo(DataType type) => false;
    public virtual bool IndexableBy(DataType type) => false;

    public virtual DataValue Convert(DataValue value) => throw new NotImplementedException();
    public virtual DataValue Cast(DataValue value) => throw new NotImplementedException();

    public virtual DataValue Add(DataValue value1, DataValue value2) => throw new NotImplementedException();
    public virtual DataValue Subtract(DataValue value1, DataValue value2) => throw new NotImplementedException();
    public virtual DataValue Multiply(DataValue value1, DataValue value2) => throw new NotImplementedException();
    public virtual DataValue Divide(DataValue value1, DataValue value2) => throw new NotImplementedException();
    public virtual DataValue Index(DataValue value, DataValue indexer) => throw new NotImplementedException();

    public Parameter? GetParameter(string name) => Parameters.FirstOrDefault(kv => kv.Value.Name == name).Value;
    //public T? GetValue<T>(string name, T? defaultValue = default) where T : notnull => Parameters.FirstOrDefault(kv => kv.Value.Name == name).Value is PrimitiveDataValue<T> p ? p.Value : defaultValue;

    public abstract bool TryParse(string input, [NotNullWhen(true)] out DataValue? value);

    public DataValue Read(ByteWindow window) => Read(window, ImmutableDictionary<Parameter, DataValue>.Empty);
    public abstract DataValue Read(ByteWindow window, IReadOnlyDictionary<Parameter, DataValue> parameters);

    public override bool Equals(object? obj) => obj is not null && obj is DataType t && Id == t.Id;
    public override int GetHashCode() => Id.GetHashCode();

    public static DataType Empty { get; } = new EmptyDataType();

    class EmptyDataType : DataType
    {
        public override string Name => "Empty";
        public override DataValue Zero => DataValue.Empty;
        public override Color Color => Color.White;
        public EmptyDataType() : base(Guid.Empty)
        {
        }

        public override DataValue Read(ByteWindow window, IReadOnlyDictionary<Parameter, DataValue> parameters) => DataValue.Empty;

        public override bool TryParse(string input, out DataValue? value)
        {
            value = null;
            return false;
        }
    }
}
