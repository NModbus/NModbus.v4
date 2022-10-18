using Microsoft.Extensions.Logging;
using NModbus.Extensions;
using NModbus.Interfaces;
using System.Net.Sockets;

namespace NModbus.Transports.TcpTransport
{
    public class ModbusTcpTransport : IModbusTransport
    {
        private readonly TcpClient tcpClient;
        private readonly ILogger<ModbusTcpTransport> logger;
        private ushort transactionIdentifier;
        private readonly object transactionIdenfitierLock = new object();

        public ModbusTcpTransport(TcpClient tcpClient, ILogger<ModbusTcpTransport> logger)
        {
            this.tcpClient = tcpClient ?? throw new ArgumentNullException(nameof(tcpClient));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<ProtocolDataUnit> SendAndReceiveAsync(byte unitNumber, ProtocolDataUnit protocolDataUnit, CancellationToken cancellationToken = default)
        {
            var transactionIdenfier = await SendAsyncInternal(unitNumber, protocolDataUnit, cancellationToken);

            var mbapHeaderBuffer = new byte[MbapHeaderSerializer.MbapHeaderLength];

            await tcpClient.GetStream().ReadBufferAsync(mbapHeaderBuffer, cancellationToken);

            var mbapHeader = MbapHeaderSerializer.DeserializeMbapHeader(mbapHeaderBuffer);

            if (transactionIdenfier != mbapHeader.TransactionIdentifier)
                throw new IOException($"The TransactionIdentier 0x{unitNumber:X4} was sent, but 0x{unitNumber:X4} was received.");

            var pduBuffer = new byte[mbapHeader.Length - 1];

            await tcpClient.GetStream().ReadBufferAsync(pduBuffer, cancellationToken);

            return new ProtocolDataUnit(pduBuffer);
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

        public async Task SendAsync(byte unitNumber, ProtocolDataUnit protocolDataUnit, CancellationToken cancellationToken = default)
        {
            await SendAsyncInternal(unitNumber, protocolDataUnit, cancellationToken);
        }

        private async Task<ushort> SendAsyncInternal(byte unitNumber, ProtocolDataUnit protocolDataUnit, CancellationToken cancellationToken = default)
        {
            //Get the next transaction id
            var transactionIdenfier = GetNextTransactionIdenfier();

            //Create the header
            var mbapHeader = MbapHeaderSerializer.SerializeMbapHeader(
                transactionIdentifier,
                (ushort)(protocolDataUnit.Length + 1),
                unitNumber);

            //Create a buffer with enough room for the whole message.
            var buffer = new byte[mbapHeader.Length + protocolDataUnit.Length];

            //Copy the header in
            Array.Copy(mbapHeader, buffer, mbapHeader.Length);

            //Copy the PDU in
            Array.Copy(protocolDataUnit.ToArray(), 0, buffer, mbapHeader.Length, protocolDataUnit.Length);

            //Write it
            await tcpClient.GetStream().WriteAsync(buffer, cancellationToken);

            return transactionIdenfier;
        }
    }
}
