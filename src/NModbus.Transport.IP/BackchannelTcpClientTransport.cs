using NModbus.Interfaces;

namespace NModbus.Transport.IP
{
    /// <summary>
    /// This is used to communicate to a connected Modbus TCP client. This is only used internally by a Modbus Server.
    /// </summary>
    internal class BackchannelTcpClientTransport : IModbusClientTransport
    {
        private readonly IModbusStream stream;
        private readonly ushort transactionIdentifier;

        public BackchannelTcpClientTransport(IModbusStream stream, ushort transactionIdentifier)
        {
            this.stream = stream ?? throw new ArgumentNullException(nameof(stream));
            this.transactionIdentifier = transactionIdentifier;
        }

        public Task<IModbusDataUnit> SendAndReceiveAsync(IModbusDataUnit message, CancellationToken cancellationToken = default)
        {
            throw new NotSupportedException("Backchannel Tcp Transport cannot SendAndReceive as a Modbus Server can only respond to requests.");
        }

        public async Task SendAsync(IModbusDataUnit message, CancellationToken cancellationToken = default)
        {
            await stream.WriteIPMessageAsync(transactionIdentifier, message, cancellationToken);
        }

        public ValueTask DisposeAsync()
        {
            return default;
        }
    }
}
