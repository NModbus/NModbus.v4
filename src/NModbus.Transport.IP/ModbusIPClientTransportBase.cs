using NModbus.Interfaces;

namespace NModbus.Transport.IP
{
    public abstract class ModbusIPClientTransportBase : IModbusClientTransport
    {
        private readonly object transactionIdenfitierLock = new();
        private ushort transactionIdentifierCounter;

        protected ushort GetNextTransactionIdenfier()
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

        public abstract Task<IModbusDataUnit> SendAndReceiveAsync(IModbusDataUnit message, CancellationToken cancellationToken = default);

        public abstract Task SendAsync(IModbusDataUnit message, CancellationToken cancellationToken = default);

        public abstract ValueTask DisposeAsync();
    }
}
