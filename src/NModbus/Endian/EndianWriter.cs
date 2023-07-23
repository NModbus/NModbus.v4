namespace NModbus.Endian
{
    public class EndianWriter : IDisposable
    {
        private readonly MemoryStream stream = new();

        public EndianWriter(Endianness endianness)
        {
            Endianness = endianness;
        }

        public Endianness Endianness { get; }

        public void Write(byte value)
        {
            stream.Write(new byte[] { value });
        }

        public void Write(ushort value)
        {
            var bytes = BitConverter.GetBytes(value);

            WriteSingletonBytes(bytes);
        }

        public void Write(byte[] value)
        {
            stream.Write(value);
        }

        /// <summary>
        /// Only call this for the number of bytes it takes to represent a single element.
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        private void WriteSingletonBytes(byte[] bytes)
        {
            if (Endianness == Endianness.BigEndian)
            {
                Array.Reverse(bytes);
            }

            stream.Write(bytes);
        }

        public byte[] ToArray()
        {
            return stream.ToArray();
        }

        public void Dispose()
        {
            stream.Dispose();
        }
    }
}
