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

        public async Task<ApplicationDataUnit> SendAndReceiveAsync(ApplicationDataUnit applicationDataUnit, CancellationToken cancellationToken = default)
        {
            await SendAsync(applicationDataUnit, cancellationToken);

            return await ReceiveAsync(cancellationToken);
            
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

        public async Task SendAsync(ApplicationDataUnit applicationDataUnit, CancellationToken cancellationToken = default)
        {
            await SendAsyncInternal(applicationDataUnit, cancellationToken);
        }

        private async Task<ushort> SendAsyncInternal(ApplicationDataUnit applicationDataUnit, CancellationToken cancellationToken = default)
        {
            //Get the next transaction id
            var transactionIdenfier = GetNextTransactionIdenfier();

            //Create the header
            var mbapHeader = MbapHeaderSerializer.SerializeMbapHeader(
                transactionIdentifier,
                (ushort)(applicationDataUnit.ProtocolDataUnit.Length + 1),
                applicationDataUnit.UnitNumber);

            //Create a buffer with enough room for the whole message.
            var buffer = new byte[mbapHeader.Length + applicationDataUnit.ProtocolDataUnit.Length];

            //Copy the header in
            Array.Copy(mbapHeader, buffer, mbapHeader.Length);

            //Copy the PDU in
            Array.Copy(applicationDataUnit.ProtocolDataUnit.ToArray(), 0, buffer, mbapHeader.Length, applicationDataUnit.ProtocolDataUnit.Length);

            //Write it
            await tcpClient.GetStream().WriteAsync(buffer, cancellationToken);

            return transactionIdenfier;
        }

        public async Task<ApplicationDataUnit> ReceiveAsync(CancellationToken cancellationToken = default)
        {
            var mbapHeaderBuffer = new byte[MbapHeaderSerializer.MbapHeaderLength];

            await tcpClient.GetStream().ReadBufferAsync(mbapHeaderBuffer, cancellationToken);

            var mbapHeader = MbapHeaderSerializer.DeserializeMbapHeader(mbapHeaderBuffer);

            //if (transactionIdenfier != mbapHeader.TransactionIdentifier)
            //    throw new IOException($"The TransactionIdentier 0x{unitNumber:X4} was sent, but 0x{unitNumber:X4} was received.");

            var pduBuffer = new byte[mbapHeader.Length - 1];

            await tcpClient.GetStream().ReadBufferAsync(pduBuffer, cancellationToken);

            return new ApplicationDataUnit(mbapHeader.UnitIdentifier, new ProtocolDataUnit(pduBuffer));
        }
    }
}
