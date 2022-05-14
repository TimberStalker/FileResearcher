using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileResearcher.Blocks.Read
{
    public class ByteWindow
    {
        byte[] data;
        int index;

        public ByteWindow(byte[] data)
        {
            this.data = data;
            index = 0;
        }

        public Span<byte> ReadBytes(int ammount)
        {
            return data.AsSpan()[index..(index+= ammount)];
        }

        public byte Read()
        {
            return data[index++];
        }
    }
}
