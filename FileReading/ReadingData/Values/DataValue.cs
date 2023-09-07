using FileReading.ReadingData.Types;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace FileReading.ReadingData.Values;

public abstract class DataValue
{
    public DataType BaseType { get; }
    public DataValue? Parent { get; protected set; }
    public string Name { get; set; } = "";
    
    public MetaData Metadata { get; }
    
    public List<DataValue> Children { get; } = new List<DataValue>();
    
    public bool IsEmpty => this is EmptyDataValue;
    public virtual void ToXml(XmlWriter writer)
    {
        writer.WriteStartElement(BaseType.Name);

        writer.WriteAttributeString("name", Name);
        writer.WriteAttributeString("value", ToString());
        foreach (var child in Children)
        {
            child.ToXml(writer);
        }

        writer.WriteEndElement();
    }

    public DataValue NextChild
    {
        set
        {
            Children.Add(value);
            value.Parent = this;
        }
    }

    public DataValue(DataType baseType)
    {
        BaseType = baseType;
        Metadata = new MetaData();
    }
    public IEnumerable<DataValue> Flatten() => new[] { this }.Concat(Children.SelectMany(c => c.Flatten()));
    public abstract DataValue ShallowClone();
    public abstract DataValue DeepClone();

    public bool ConvertableTo(DataType type) => type.ConvertableFrom(BaseType);
    public bool CastableTo(DataType type) => type.CastableFrom(BaseType);
    public bool IndexableBy(DataType type) => type.IndexableBy(BaseType);

    public bool AddableTo(DataType type) => BaseType.AddableTo(type);
    public bool SubtractibleTo(DataType type) => BaseType.SubtractibleTo(type);
    public bool MultipliableTo(DataType type) => BaseType.MultipliableTo(type);
    public bool DivisibleTo(DataType type) => BaseType.DivisibleTo(type);

    public bool AddableTo(DataValue value) => BaseType.AddableTo(value.BaseType);
    public bool SubtractibleTo(DataValue value) => BaseType.SubtractibleTo(value.BaseType);
    public bool MultipliableTo(DataValue value) => BaseType.MultipliableTo(value.BaseType);
    public bool DivisibleTo(DataValue value) => BaseType.DivisibleTo(value.BaseType);

    public DataValue Convert(DataType type) => type.Convert(this);
    public DataValue Cast(DataType type) => type.Cast(this);
    public DataValue Index(DataValue indexer) => BaseType.Index(this, indexer);

    public DataValue Add(DataValue value) => BaseType.Add(this, value);
    public DataValue Subtract(DataValue value) => BaseType.Subtract(this, value);
    public DataValue Multiply(DataValue value) => BaseType.Multiply(this, value);
    public DataValue Divide(DataValue value) => BaseType.Divide(this, value);

    public virtual string ToString(string format) => ToString()!;
    public abstract bool ValueEquals(DataValue other);
    public static DataValue Empty => new EmptyDataValue();

    class EmptyDataValue : DataValue
    {
        public EmptyDataValue() : base(DataType.Empty)
        {
        }
        public override string ToString() => "Empty";
        public override DataValue ShallowClone()
        {
            var result = new EmptyDataValue(); 
            result.Name = Name;
            result.Parent = Parent;

            result.Children.AddRange(Children);

            foreach (var (key, value) in Metadata)
            {
                result.Metadata.Set(key, value);
            }
            return result;
        }
        public override DataValue DeepClone()
        {
            var result = new EmptyDataValue(); 
            result.Name = Name;
            result.Parent = Parent;

            result.Children.AddRange(Children.Select(v => v.DeepClone()));

            foreach (var (key, value) in Metadata)
            {
                result.Metadata.Set(key, value);
            }
            return result;
        }
        public override bool ValueEquals(DataValue other) => false;
    }

    public class MetaData : IEnumerable<KeyValuePair<string, object>> 
    {
        Dictionary<string, object> metadata;
        public MetaData(params (string, object)[] metadata)
        {
            this.metadata = metadata.ToDictionary(k => k.Item1, v => v.Item2);
        }
        public void Set<T>(string name, T value) where T : notnull => metadata[name] = value;
        public T Get<T>(string name) => (T)metadata[name];
        public bool TryGet<T>(string name, [NotNullWhen(true)] out T value) 
        {
            if(metadata.TryGetValue(name, out var v))
            {
                if(v is T val)
                {
                    value = val;
                    return true;
                }
            }
            value = default;
            return false;
        }


        public IEnumerator<KeyValuePair<string, object>> GetEnumerator() => metadata.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
