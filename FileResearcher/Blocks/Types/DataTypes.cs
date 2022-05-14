using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileResearcher.Blocks.Read;
using FileResearcher.Utils;

namespace FileResearcher.Blocks.Types
{
    public static class DataTypes
    {
        public static Dictionary<SharedResource<string>, DataType> GetPrimitives(ResourcePool<string, DataType> resourcePool)
        {
            var list = new Dictionary<SharedResource<string>, DataType>();

            var intType = new DataType("int", resourcePool);
            var intRead = new PrimitiveRead(intType, (window) => BitConverter.ToInt32(window.ReadBytes(4)));
            intType.reads.Add(intRead);

            var byteType = new DataType("byte", resourcePool);
            var byteRead = new PrimitiveRead(byteType, (window) => window.Read());
            byteType.reads.Add(byteRead);

            var uIntType = new DataType("uint", resourcePool);
            var uIntRead = new PrimitiveRead(uIntType, (window) => BitConverter.ToUInt32(window.ReadBytes(4)));
            uIntType.reads.Add(uIntRead);

            var longType = new DataType("long", resourcePool);
            var longRead = new PrimitiveRead(longType, (window) => BitConverter.ToInt64(window.ReadBytes(4)));
            longType.reads.Add(longRead);

            var floatType = new DataType("float", resourcePool);
            var floatRead = new PrimitiveRead(floatType, (window) => BitConverter.ToSingle(window.ReadBytes(4)));
            floatType.reads.Add(floatRead);

            var charType = new DataType("char", resourcePool);
            var charRead = new PrimitiveRead(charType, (window) => BitConverter.ToChar(window.ReadBytes(1)));
            charType.reads.Add(charRead);

            var stringType = new DataType("string", resourcePool);
            var stringRead = new PrimitiveRead(stringType,
                (window) =>
                {
                    int length = BitConverter.ToInt32(window.ReadBytes(1));
                    return BitConverter.ToString(window.ReadBytes(length).ToArray());
                });
            stringType.reads.Add(stringRead);

            list.Add(intType.Name, intType);

            return list;
        }
    }
}
