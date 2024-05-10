using NModbus.Interfaces;

namespace NModbus.Transport.IP
{
    public abstract class ModbusIPClientTransportBase : IModbusClientTransport
    {
        private readonly object _transactionIdentifierLock = new();
        private ushort _transactionIdentifierCounter;

        protected ushort GetNextTransactionIdentifier()
        {
            ushort transactionIdentifier;

            lock (_transactionIdentifierLock)
            {
                unchecked
                {
                    _transactionIdentifierCounter++;

                    transactionIdentifier = _transactionIdentifierCounter;
                }
            }

            return transactionIdentifier;
        }

        public abstract Task<IModbusDataUnit> SendAndReceiveAsync(IModbusDataUnit message, CancellationToken cancellationToken = default);

        public abstract Task SendAsync(IModbusDataUnit message, CancellationToken cancellationToken = default);

        public abstract ValueTask DisposeAsync();
    }
}
