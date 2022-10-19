using NModbus.Endian;

namespace NModbus.Transports.TcpTransport
{
    public static class MbapHeaderSerializer
    {
        public const ushort ProtocolIdentifier = 0x0000;

        /// <summary>
        /// The length of a MBAP header.
        /// </summary>
        public const ushort MbapHeaderLength = 7;

        public static byte[] SerializeMbapHeader(
            ushort transactionIdentifier, 
            ushort length, 
            byte unitIdentifier)
        {
            using var writer = new EndianWriter(Endianness.BigEndian);

            writer.Write(transactionIdentifier);
            writer.Write(ProtocolIdentifier);
            writer.Write(length);
            writer.Write(unitIdentifier);

            return writer.ToArray();
        }

        public static MbapHeader DeserializeMbapHeader(
            byte[] buffer)
        {
            if (buffer.Length != MbapHeaderLength)
                throw new InvalidOperationException($"Expected a buffer of size {MbapHeaderLength} but was given a buffer with {buffer.Length} elements.");

            using var reader = new EndianReader(buffer, Endianness.BigEndian);

            return new MbapHeader(
                reader.ReadUInt16(),
                reader.ReadUInt16(),
                reader.ReadUInt16(),
                reader.ReadByte());
        }
    }
}
