using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileReading.Reading;
using FileReading.Utils;
using FileReading.Values;

namespace FileReading.Trees
{
    public class ReadTreeValue : ReadTreeNode
    {
        Read read;
        Dictionary<SharedResource<string>, ReadParameterValue> parameters;
        public ReadTreeValue(Read read)
        {
            parameters = new Dictionary<SharedResource<string>, ReadParameterValue>();
            this.read = read;
            SetRead(read);
        }
        public void SetRead(Read read)
        {
            this.read = read;
        }
        public override DataValue Read(ByteStream byteStream)
        {
            Dictionary<SharedResource<string>, DataValue> parameterValues = new();
            foreach (var (key, param) in parameters)
            {
                var value = param.GetValue();
                parameterValues.Add(key, value);
            }
            return read.ReadType(byteStream, parameterValues);
        }
    }
}
