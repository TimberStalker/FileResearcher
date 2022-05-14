using FileResearcher.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using FileResearcher.Blocks.Read;
using System.Collections;

namespace FileResearcher.Blocks.Types;

public class DataType : IXmlSerializable, IEnumerable<KeyValuePair<ResourcePool<string, DataType>.UniqueResource, DataType>>
{
    public ResourcePool<string, DataType>.UniqueResource Name { get; set; }


    protected ResourcePool<string, DataType> BaseTypes { get; set; }

    public List<Read.Read> reads { get; set; }

    public DataType this[ResourcePool<string, DataType>.UniqueResource resource] => BaseTypes[resource];
    public DataType this[string key] => BaseTypes[key];

    public DataType(ResourcePool<string, DataType>.UniqueResource name)
    {
        Name = name;

        BaseTypes = new ResourcePool<string, DataType>();

        reads = new List<Read.Read> { };
    }

    public bool TryAddBase(string name, DataType baseType)
    {
        return BaseTypes.TryAdd(name, baseType, out var resource);
    }

    public DataType? GetBaseType(ResourcePool<string, DataType>.UniqueResource resource)
    {
        if(BaseTypes?.TryGetValue(resource, out DataType? baseType) == true)
        {
            return baseType;
        }
        return null;
    }
    public DataType? GetBaseType(string key)
    {
        if (BaseTypes?.TryGetValue(key, out DataType? baseType) == true)
        {
            return baseType;
        }
        return null;
    }

    public virtual DataValue CreateValue()
    {
        var value = new DataValue(this);

        if (BaseTypes is not null)
        {
            foreach (var (name, dataType) in BaseTypes)
            {
                value.Add(name, dataType.CreateValue());
            }
        }

        return value;
    }

    public XmlSchema? GetSchema()
    {
        throw new NotImplementedException();
    }

    public void ReadXml(XmlReader reader)
    {
        throw new NotImplementedException();
    }

    public void WriteXml(XmlWriter writer)
    {
        writer.WriteStartElement((string)Name);

        if(BaseTypes != null)
        {
            foreach(var (name, baseType) in BaseTypes)
            {
                writer.WriteStartElement(baseType.Name.Value);
                writer.WriteAttributeString("name", (string)name);
                writer.WriteEndElement();
            }
        }

        writer.WriteEndElement();
    }

    public IEnumerator<KeyValuePair<ResourcePool<string, DataType>.UniqueResource, DataType>> GetEnumerator()
    {
        return BaseTypes.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}