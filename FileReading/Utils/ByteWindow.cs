using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileReading.Utils;

public class ByteWindow
{
    int position;
    byte[] buffer;
    public int Position => position;
    public int Size => buffer.Length;
    public ByteWindow(byte[] buffer)
    {
        position = 0;
        this.buffer = buffer;
    }

    public ByteRead ReadBytes(int count)
    {
        var range = position..(position += count);
        return new ByteRead(buffer[range], range);
    }

    public ByteRead<T> ReadBytes<T>(int length, Func<byte[], int, T> function)
    {
        int start = position;
        var result = function(buffer, position);
        position += length;
        return new ByteRead<T>(result, start..position);
    }

    public ref struct ByteRead
    {
        public readonly ReadOnlySpan<byte> bytes;
        public readonly Range range;

        public ByteRead(ReadOnlySpan<byte> bytes, Range range)
        {
            this.bytes = bytes;
            this.range = range;
        }

        public static implicit operator ReadOnlySpan<byte>(ByteRead read)
        {
            return read.bytes;
        }
    }
    public ref struct ByteRead<T>
    {
        public readonly T value;
        public readonly Range range;

        public ByteRead(T value, Range range)
        {
            this.value = value;
            this.range = range;
        }

        public static implicit operator T(ByteRead<T> read)
        {
            return read.value;
        }
    }
}
