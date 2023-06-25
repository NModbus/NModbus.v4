using Microsoft.Extensions.Logging;
using NModbus.Interfaces;
using System.Net.Sockets;

namespace NModbus.Transports.TcpTransport
{
    public class ModbusTcpClientTransport : IModbusClientTransport
    {
        private readonly TcpClient tcpClient;
        private readonly ILogger<ModbusTcpClientTransport> logger;
        private ushort transactionIdentifier;
        private readonly object transactionIdenfitierLock = new object();
        private readonly Stream stream;

        public ModbusTcpClientTransport(
            TcpClient tcpClient, 
            ILoggerFactory loggerFactory) 
        {
            if (loggerFactory is null)
                throw new ArgumentNullException(nameof(loggerFactory));

            this.tcpClient = tcpClient ?? throw new ArgumentNullException(nameof(tcpClient));
            this.logger = loggerFactory.CreateLogger<ModbusTcpClientTransport>();
            stream = this.tcpClient.GetStream();
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
            await SendInternalAsync(applicationDataUnit, cancellationToken);
        }

        private async Task<ushort> SendInternalAsync(ApplicationDataUnit applicationDataUnit, CancellationToken cancellationToken = default)
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
            await stream.WriteAsync(buffer, cancellationToken);

            return transactionIdenfier;
        }

        public async Task<ApplicationDataUnit> ReceiveAsync(CancellationToken cancellationToken = default)
        {
            return await stream.ReceiveApplicationDataUnitFromTcpStream(cancellationToken);
        }

        public ValueTask DisposeAsync()
        {
            tcpClient?.Dispose();

            return ValueTask.CompletedTask;
        }
    }
}
