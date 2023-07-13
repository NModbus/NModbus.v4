using Microsoft.Extensions.Logging;
using NModbus.Interfaces;

namespace NModbus.Transport.Tcp
{
    public class ModbusTcpClientTransport : IModbusClientTransport
    {
        private readonly ILogger<ModbusTcpClientTransport> logger;
        private ushort transactionIdentifier;
        private readonly object transactionIdenfitierLock = new object();
        private readonly ITcpClientLifetime tcpClientStrategy;

        public ModbusTcpClientTransport(ITcpClientLifetime tcpClientStrategy,
            ILoggerFactory loggerFactory)
        {
            if (loggerFactory is null)
                throw new ArgumentNullException(nameof(loggerFactory));

            logger = loggerFactory.CreateLogger<ModbusTcpClientTransport>();
            this.tcpClientStrategy = tcpClientStrategy ?? throw new ArgumentNullException(nameof(tcpClientStrategy));
        }

        public async Task<IModbusMessage> SendAndReceiveAsync(IModbusMessage message, CancellationToken cancellationToken = default)
        {
            await using var tcpClientContainer = await tcpClientStrategy.GetTcpClientAsync(cancellationToken)
                .ConfigureAwait(false);

            await SendInternalAsync(tcpClientContainer.Stream, message, cancellationToken);

            return await tcpClientContainer.Stream.ReadTcpMessage(cancellationToken);

        }

        private ushort GetNextTransactionIdenfier()
        {
            ushort result;

            lock (transactionIdenfitierLock)
            {
                unchecked
                {
                    transactionIdentifier++;

                    result = transactionIdentifier;
                }
            }

            return result;
        }

        public async Task SendAsync(IModbusMessage message, CancellationToken cancellationToken = default)
        {
            await using var tcpClientContainer = await tcpClientStrategy.GetTcpClientAsync(cancellationToken)
               .ConfigureAwait(false);

            await SendInternalAsync(tcpClientContainer.Stream, message, cancellationToken);
        }

        private async Task<ushort> SendInternalAsync(Stream stream, IModbusMessage message, CancellationToken cancellationToken = default)
        {
            //Get the next transaction id
            var transactionIdenfier = GetNextTransactionIdenfier();

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

            return transactionIdenfier;
        }

        public async ValueTask DisposeAsync()
        {
            await tcpClientStrategy.DisposeAsync();
        }
    }
}
