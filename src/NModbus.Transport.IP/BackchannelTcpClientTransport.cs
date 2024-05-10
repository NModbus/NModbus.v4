using NModbus.Interfaces;

namespace NModbus.Transport.IP
{
    /// <summary>
    /// This is used to communicate to a connected Modbus TCP client. This is only used internally by a Modbus Server.
    /// </summary>
    internal class BackchannelTcpClientTransport : IModbusClientTransport
    {
        private readonly IModbusStream _stream;
        private readonly ushort _transactionIdentifier;

        public BackchannelTcpClientTransport(IModbusStream stream, ushort transactionIdentifier)
        {
            _stream = stream ?? throw new ArgumentNullException(nameof(stream));
            _transactionIdentifier = transactionIdentifier;
        }

        public Task<IModbusDataUnit> SendAndReceiveAsync(IModbusDataUnit message, CancellationToken cancellationToken = default)
        {
            throw new NotSupportedException("Backchannel Tcp Transport cannot SendAndReceive as a Modbus Server can only respond to requests.");
        }

        public async Task SendAsync(IModbusDataUnit message, CancellationToken cancellationToken = default)
        {
            await _stream.WriteIpMessageAsync(_transactionIdentifier, message, cancellationToken);
        }

        public ValueTask DisposeAsync()
        {
            return default;
        }
    }
}
