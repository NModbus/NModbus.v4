using Microsoft.Extensions.Logging;
using NModbus.Interfaces;

namespace NModbus.Transport.Tcp
{
    public class ModbusTcpClientTransport : ModbusTcpClientTransportBase
    {
        private readonly ILogger<ModbusTcpClientTransport> logger;
        private readonly ITcpClientConnectionStrategy tcpClientStrategy;
        private readonly object transactionIdenfitierLock = new object();

        private ushort transactionIdentifierCounter;

        public ModbusTcpClientTransport(ITcpClientConnectionStrategy tcpClientStrategy,
            ILoggerFactory loggerFactory)
        {
            if (loggerFactory is null)
                throw new ArgumentNullException(nameof(loggerFactory));

            logger = loggerFactory.CreateLogger<ModbusTcpClientTransport>();
            this.tcpClientStrategy = tcpClientStrategy ?? throw new ArgumentNullException(nameof(tcpClientStrategy));
        }

        private ushort GetNextTransactionIdenfier()
        {
            ushort transactionIdentifier;

            lock (transactionIdenfitierLock)
            {
                unchecked
                {
                    transactionIdentifierCounter++;

                    transactionIdentifier = transactionIdentifierCounter;
                }
            }

            return transactionIdentifier;
        }

        public override async Task<IModbusMessage> SendAndReceiveAsync(IModbusMessage message, CancellationToken cancellationToken = default)
        {
            await using var tcpClientContainer = await tcpClientStrategy.GetTcpClientAsync(cancellationToken)
                .ConfigureAwait(false);

            var transactionIdentifier = GetNextTransactionIdenfier();

            await SendProtectedAsync(tcpClientContainer.TcpClient.GetStream(), transactionIdentifier, message, cancellationToken);

            var receivedMessage = await tcpClientContainer.TcpClient.GetStream().ReadTcpMessageAsync(cancellationToken);

            if (receivedMessage.Header.TransactionIdentifier != transactionIdentifier)
                throw new InvalidOperationException($"TransactionIdentifier {transactionIdentifier}");

            return receivedMessage;
        }

        public override async Task SendAsync(IModbusMessage message, CancellationToken cancellationToken = default)
        {
            var transactionIdentifier = GetNextTransactionIdenfier();

            await using var tcpClientContainer = await tcpClientStrategy.GetTcpClientAsync(cancellationToken)
               .ConfigureAwait(false);

            await SendProtectedAsync(tcpClientContainer.TcpClient.GetStream(), transactionIdentifier, message, cancellationToken);
        }

        public override async ValueTask DisposeAsync()
        {
            await tcpClientStrategy.DisposeAsync();
        }
    }
}
