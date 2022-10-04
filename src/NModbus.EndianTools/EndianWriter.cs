namespace NModbus.EndianTools
{
    public class EndianWriter
    {
        public EndianWriter(Stream stream, Endianness endianness)
        {
            Stream = stream ?? throw new ArgumentNullException(nameof(stream));
            Endianness = endianness;
        }

        protected Stream Stream { get; }

        public Endianness Endianness { get; }

        public async Task WriteAsync(byte value, CancellationToken cancellationToken = default)
        {
            await Stream.WriteAsync(new byte[] { value }, cancellationToken);
        }

        public async Task WriteAsync(ushort value, CancellationToken cancellationToken = default)
        {
            var bytes = BitConverter.GetBytes(value);

            await WriteBytesAsync(bytes, cancellationToken);
        }

        private async Task WriteBytesAsync(byte[] bytes, CancellationToken cancellationToken)
        {
            if (Endianness == Endianness.BigEndian)
            {
                Array.Reverse(bytes);
            }

            await Stream.WriteAsync(bytes, cancellationToken);
        }
    }
}
