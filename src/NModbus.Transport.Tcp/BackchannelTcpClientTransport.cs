using NModbus.Interfaces;

namespace NModbus.Transport.Tcp
{
    /// <summary>
    /// This is used to communicate to a connected Modbus TCP client. This is only used internally by a Modbus Server.
    /// </summary>
    internal class BackchannelTcpClientTransport : ModbusTcpClientTransportBase
    {
        private readonly Stream stream;
        private readonly ushort transactionIdentifier;

        public BackchannelTcpClientTransport(Stream stream, ushort transactionIdentifier)
        {
            this.stream = stream ?? throw new ArgumentNullException(nameof(stream));
            this.transactionIdentifier = transactionIdentifier;
        }

        public override Task<IModbusMessage> SendAndReceiveAsync(IModbusMessage message, CancellationToken cancellationToken = default)
        {
            throw new NotSupportedException("Backchannel Tcp Transport cannot SendAndReceive as a Modbus Server can only respond to requests.");
        }

        public override async Task SendAsync(IModbusMessage message, CancellationToken cancellationToken = default)
        {
            await SendProtectedAsync(stream, transactionIdentifier, message, cancellationToken);
        }

        public override ValueTask DisposeAsync()
        {
            return default;
        }
    }
}
