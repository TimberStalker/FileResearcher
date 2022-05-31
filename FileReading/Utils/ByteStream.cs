using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileReading.Utils
{
    public class ByteStream
    {
        byte[] data;
        int i;
        public ByteStream(byte[] data, int start)
        {
            this.data = data;
            i = start;
        }

        public byte Read()
        {
            return data[i++];
        }

        public Span<byte> Read(int ammount)
        {
            var span = new Span<byte>(data, i, ammount);
            i += ammount;
            return span;
        }
    }
}
