using Microsoft.Extensions.Logging;
using NModbus.Interfaces;

namespace NModbus.Transport.IP
{
    public class ModbusIPClientTransport : ModbusIPClientTransportBase
    {
        private readonly ILogger<ModbusIPClientTransport> logger;
        private readonly IConnectionStrategy connectionStrategy;

        public ModbusIPClientTransport(IConnectionStrategy connectionStrategy,
            ILoggerFactory loggerFactory)
        {
            if (loggerFactory is null)
                throw new ArgumentNullException(nameof(loggerFactory));

            logger = loggerFactory.CreateLogger<ModbusIPClientTransport>();
            this.connectionStrategy = connectionStrategy ?? throw new ArgumentNullException(nameof(connectionStrategy));
        }

        public override async Task<IModbusDataUnit> SendAndReceiveAsync(IModbusDataUnit message, CancellationToken cancellationToken = default)
        {
            await using var streamContainer = await connectionStrategy.GetStreamContainer(cancellationToken)
                .ConfigureAwait(false);

            var transactionIdentifier = GetNextTransactionIdenfier();

            await streamContainer.Stream.WriteIPMessageAsync(transactionIdentifier, message, cancellationToken);

            var receivedMessage = await streamContainer.Stream.ReadIPMessageAsync(cancellationToken);

            if (receivedMessage.Header.TransactionIdentifier != transactionIdentifier)
                throw new InvalidOperationException($"TransactionIdentifier {transactionIdentifier}");

            return receivedMessage;
        }

        public override async Task SendAsync(IModbusDataUnit message, CancellationToken cancellationToken = default)
        {
            var transactionIdentifier = GetNextTransactionIdenfier();

            await using var streamContainer = await connectionStrategy.GetStreamContainer(cancellationToken)
               .ConfigureAwait(false);

            await streamContainer.Stream.WriteIPMessageAsync(transactionIdentifier, message, cancellationToken);
        }

        public override async ValueTask DisposeAsync()
        {
            await connectionStrategy.DisposeAsync();

            GC.SuppressFinalize(this);
        }
    }
}
