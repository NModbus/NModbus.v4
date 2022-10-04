namespace NModbus.EndianTools
{
    public class EndianReader
    {
        public EndianReader(Stream stream, Endianness endianness)
        {
            Stream = stream ?? throw new ArgumentNullException(nameof(stream));
            Endianness = endianness;
        }

        protected Stream Stream { get; }

        public Endianness Endianness { get; }

        public async Task<byte> ReadByteAsync(CancellationToken cancellationToken = default)
        {
            var buffer = new byte[1];

            var numberRead = await Stream.ReadAsync(buffer, 0, 1);

            if (numberRead != 1)
            {
                throw new InvalidOperationException($"Expected 1 bytes but got {numberRead} instead.");
            }

            return buffer[0];
        }

        public async Task<ushort> ReadUInt16Async(CancellationToken cancellationToken = default)
        {
            var bytes = await ReadBytesAsync(sizeof(ushort), cancellationToken);

            return BitConverter.ToUInt16(bytes);
        }

        private async Task<byte[]> ReadBytesAsync(int count, CancellationToken cancellationToken = default)
        {
            var buffer = new byte[count];

            var numberRead = await Stream.ReadAsync(buffer, 0, count);

            if (numberRead != count)
            {
                throw new InvalidOperationException($"Expected {count} bytes but got {numberRead} instead.");
            }

            if (Endianness == Endianness.BigEndian)
            {
                Array.Reverse(buffer);
            }

            return buffer;
        }
    }
}
