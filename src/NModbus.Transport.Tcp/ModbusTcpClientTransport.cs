using Microsoft.Extensions.Logging;
using NModbus.Interfaces;

namespace NModbus.Transport.Tcp
{
    public class ModbusTcpClientTransport : ModbusTcpClientTransportBase
    {
        private readonly ILogger<ModbusTcpClientTransport> logger;
        private readonly ITcpClientConnectionStrategy tcpClientStrategy;
        private readonly object transactionIdenfitierLock = new();

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

        public override async Task<IModbusDataUnit> SendAndReceiveAsync(IModbusDataUnit message, CancellationToken cancellationToken = default)
        {
            await using var tcpClientContainer = await tcpClientStrategy.GetTcpClientAsync(cancellationToken)
                .ConfigureAwait(false);

            var transactionIdentifier = GetNextTransactionIdenfier();

            await SendProtectedAsync(tcpClientContainer.Stream, transactionIdentifier, message, cancellationToken);

            var receivedMessage = await tcpClientContainer.Stream.ReadTcpMessageAsync(cancellationToken);

            if (receivedMessage.Header.TransactionIdentifier != transactionIdentifier)
                throw new InvalidOperationException($"TransactionIdentifier {transactionIdentifier}");

            return receivedMessage;
        }

        public override async Task SendAsync(IModbusDataUnit message, CancellationToken cancellationToken = default)
        {
            var transactionIdentifier = GetNextTransactionIdenfier();

            await using var tcpClientContainer = await tcpClientStrategy.GetTcpClientAsync(cancellationToken)
               .ConfigureAwait(false);

            await SendProtectedAsync(tcpClientContainer.Stream, transactionIdentifier, message, cancellationToken);
        }

        public override async ValueTask DisposeAsync()
        {
            await tcpClientStrategy.DisposeAsync();
        }
    }
}
