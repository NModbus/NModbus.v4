using NModbus.Extensions;
using NModbus.Interfaces;

namespace NModbus.Transports.TcpTransport
{
    internal static class StreamExtensions
    {
        internal static async Task<ApplicationDataUnit> ReceiveApplicationDataUnitFromTcpStream(this Stream stream, CancellationToken cancellationToken)
        {
            var mbapHeaderBuffer = new byte[MbapHeaderSerializer.MbapHeaderLength];

            if (!await stream.TryReadBufferAsync(mbapHeaderBuffer, cancellationToken))
                return null;

            var mbapHeader = MbapHeaderSerializer.DeserializeMbapHeader(mbapHeaderBuffer);

            //if (transactionIdenfier != mbapHeader.TransactionIdentifier)
            //    throw new IOException($"The TransactionIdentier 0x{unitNumber:X4} was sent, but 0x{unitNumber:X4} was received.");

            var pduBuffer = new byte[mbapHeader.Length - 1];

            if (!await stream.TryReadBufferAsync(pduBuffer, cancellationToken))
                return null;

            return new ApplicationDataUnit(mbapHeader.UnitIdentifier, new ProtocolDataUnit(pduBuffer));
        }
    }
}
