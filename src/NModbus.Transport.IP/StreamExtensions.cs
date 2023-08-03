using NModbus.Extensions;
using NModbus.Interfaces;
using NModbus.Transport.IP.Mbap;

namespace NModbus.Transport.Tcp
{
    internal static class StreamExtensions
    {
        internal static async Task<ModbusTcpMessage> ReadTcpMessageAsync(this Stream stream, CancellationToken cancellationToken)
        {
            var mbapHeaderBuffer = new byte[MbapSerializer.MbapHeaderLength];

            if (!await stream.TryReadBufferAsync(mbapHeaderBuffer, cancellationToken))
                return null;

            var mbapHeader = MbapSerializer.DeserializeMbapHeader(mbapHeaderBuffer);

            var pduBuffer = new byte[mbapHeader.Length - 1];

            if (!await stream.TryReadBufferAsync(pduBuffer, cancellationToken))
                return null;

            return new ModbusTcpMessage(mbapHeader, new ProtocolDataUnit(pduBuffer));
        }
    }
}
