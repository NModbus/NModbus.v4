using Microsoft.Extensions.Logging;
using NModbus.Interfaces;
using System.Net.Security;
using System.Net.Sockets;

namespace NModbus.Transport.Tcp
{
    /// <summary>
    /// This represents a connection from a client on a server.
    /// </summary>
    internal class ModbusServerTcpConnection : IAsyncDisposable
    {
        private readonly TcpClient tcpClient;
        private readonly IModbusServerNetwork serverNetwork;
        private readonly Stream stream;
        private readonly CancellationTokenSource cancellationTokenSource = new();
        private readonly Task listenTask;
        private readonly ILogger logger;
        private readonly int connectionId;

        private static int connectionIdSource;

        public event EventHandler<TcpConnectionEventArgs> ConnectionClosed;

        internal ModbusServerTcpConnection(
            TcpClient tcpClient,
            IModbusServerNetwork serverNetwork,
            ILoggerFactory loggerFactory,
            ModbusTcpServerOptions options)
        {
            connectionId = Interlocked.Increment(ref connectionIdSource);

            if (loggerFactory is null)
                throw new ArgumentNullException(nameof(loggerFactory));

            logger = loggerFactory.CreateLogger<ModbusServerTcpConnection>();
            this.tcpClient = tcpClient ?? throw new ArgumentNullException(nameof(tcpClient));
            this.serverNetwork = serverNetwork;
            if (options == null) throw new ArgumentNullException(nameof(options));

            var stream = tcpClient.GetStream();

            if (options.Certificate == null)
            {
                this.stream = stream;
            }
            else
            {
                var sslStream = new SslStream(stream, false, options.ClientCertificateValidation);

                sslStream.AuthenticateAsServer(
                    options.Certificate, 
                    options.ClientCertificateRequired, 
                    options.SslProtocols, 
                    options.CheckCertificateRevocation);

                options.ConfigureSslStream?.Invoke(sslStream);

                this.stream = sslStream;
            }

            listenTask = Task.Run(() => ListenAsync(cancellationTokenSource.Token));
        }

        private async Task ListenAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var requestMessage = await stream.ReadTcpMessageAsync(cancellationToken);

                if (requestMessage == null)
                {
                    logger.LogInformation("{ConnectionId} closed after receiving 0 bytes.", connectionId);
                    OnConnectionClosed();
                    return;
                }

                logger.LogInformation("{ConnectionId} ModbusServerTcpConnection received ADU for unit {UnitIdentifier} with PDU FunctionCode {FunctionCode}.",
                    connectionId,
                    requestMessage.UnitIdentifier,
                    requestMessage.ProtocolDataUnit.FunctionCode);

                await using (var backchannelTransport = new BackchannelTcpClientTransport(stream, requestMessage.Header.TransactionIdentifier))
                {
                    await serverNetwork.ProcessRequestAsync(requestMessage, backchannelTransport, cancellationToken);
                }
            }
        }

        protected void OnConnectionClosed()
        {
            ConnectionClosed?.Invoke(this, new TcpConnectionEventArgs(tcpClient.Client.RemoteEndPoint.ToString()));
        }

        public async ValueTask DisposeAsync()
        {
            cancellationTokenSource.Dispose();

            await listenTask;
        }
    }
}
