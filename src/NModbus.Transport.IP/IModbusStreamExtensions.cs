using NModbus.Extensions;
using NModbus.Interfaces;
using NModbus.Transport.IP.Mbap;

namespace NModbus.Transport.IP
{
    internal static class IModbusStreamExtensions
    {
        internal static async Task<ModbusIPMessage> ReadIPMessageAsync(
            this IModbusStream stream, 
            CancellationToken cancellationToken = default)
        {
            var mbapHeaderBuffer = new byte[MbapSerializer.MbapHeaderLength];

            if (!await stream.TryReadBufferAsync(mbapHeaderBuffer, cancellationToken))
                return null;

            var mbapHeader = MbapSerializer.DeserializeMbapHeader(mbapHeaderBuffer);

            var pduBuffer = new byte[mbapHeader.Length - 1];

            if (!await stream.TryReadBufferAsync(pduBuffer, cancellationToken))
                return null;

            return new ModbusIPMessage(mbapHeader, new ProtocolDataUnit(pduBuffer));
        }

        internal static async Task WriteIPMessageAsync(
            this IModbusStream stream,
            ushort transactionIdentifier,
            IModbusDataUnit message,
            CancellationToken cancellationToken = default)
        {
            var buffer = message.Serialize(transactionIdentifier);

            //Write it
            await stream.WriteAsync(buffer, 0, buffer.Length, cancellationToken);
        }
    }
}
