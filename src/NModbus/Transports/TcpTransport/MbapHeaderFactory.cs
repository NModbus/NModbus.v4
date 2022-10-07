using NModbus.EndianTools;
using System.Reflection;

namespace NModbus.Transports.TcpTransport
{
    public static class MbapHeaderFactory
    {
        private const ushort ProtocolIdentifier = 0x0000;

        public const ushort MbapHeaderLength = 7;

        public static async Task<byte[]> CreateMbapHeaderAsync(
            ushort transactionIdentifier, 
            ushort length, 
            byte unitIdentifier, 
            CancellationToken cancellationToken = default)
        {
            using var stream = new MemoryStream();

            var endianWriter = new EndianWriter(stream, Endianness.BigEndian);

            await endianWriter.WriteAsync(transactionIdentifier, cancellationToken);
            await endianWriter.WriteAsync(ProtocolIdentifier, cancellationToken);
            await endianWriter.WriteAsync(length, cancellationToken);
            await endianWriter.WriteAsync(unitIdentifier, cancellationToken);

            return stream.ToArray();
        }

        public static async Task<MbapHeader> ParseMbapHeaderAsync(
            byte[] buffer,
            CancellationToken cancellationToken)
        {
            if (buffer.Length != MbapHeaderLength)
                throw new InvalidOperationException($"Expected a buffer of size {MbapHeaderLength} but was given a buffer with {buffer.Length} elements.");

            using var stream = new MemoryStream(buffer);

            var reader = new EndianReader(stream, Endianness.BigEndian);

            return new MbapHeader
            {
                TransactionIdentifier = await reader.ReadUInt16Async(cancellationToken),
                ProtocolIdentifier = await reader.ReadUInt16Async(cancellationToken),
                Length = await reader.ReadUInt16Async(cancellationToken),
                UnitIdentifier = await reader.ReadByteAsync(cancellationToken)
            };
        }
    }
}
