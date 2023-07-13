using NModbus.Interfaces;
using NModbus.Transport.Tcp.TcpMessages;

namespace NModbus.Transport.Tcp
{
    /// <summary>
    /// This is the transport from the client point of view.
    /// </summary>
    public abstract class ModbusTcpClientTransportBase : IModbusClientTransport
    {
        public abstract Task<IModbusMessage> SendAndReceiveAsync(IModbusMessage message, CancellationToken cancellationToken = default);

        public abstract Task SendAsync(IModbusMessage message, CancellationToken cancellationToken = default);

        protected async Task SendProtectedAsync(
            Stream stream, 
            ushort transactionIdentifier, 
            IModbusMessage message, 
            CancellationToken cancellationToken = default)
        {
            //Create the header
            var mbapHeader = MbapHeaderSerializer.SerializeMbapHeader(
                transactionIdentifier,
                (ushort)(message.ProtocolDataUnit.Length + 1),
                message.UnitIdentifier);

            //Create a buffer with enough room for the whole message.
            var buffer = new byte[mbapHeader.Length + message.ProtocolDataUnit.Length];

            //Copy the header in
            Array.Copy(mbapHeader, buffer, mbapHeader.Length);

            //Copy the PDU in
            Array.Copy(message.ProtocolDataUnit.ToArray(), 0, buffer, mbapHeader.Length, message.ProtocolDataUnit.Length);

            //Write it
            await stream.WriteAsync(buffer, cancellationToken);
        }

        public abstract ValueTask DisposeAsync();
    }
}
