using Microsoft.Extensions.Logging;
using NModbus.Interfaces;
using System.Net.Security;
using System.Net.Sockets;

namespace NModbus.Transport.IP
{
    /// <summary>
    /// This represents a connection from a client on a server.
    /// </summary>
    internal class ModbusServerTcpConnection : IAsyncDisposable
    {
        private readonly TcpClient tcpClient;
        private readonly IModbusServerNetwork serverNetwork;
        private readonly SslServerAuthenticationOptions options;
        private readonly CancellationTokenSource cancellationTokenSource = new();
        private Task listenTask;
        private readonly ILogger logger;
        private readonly int connectionId;

        private IModbusStream stream;
        private static int connectionIdSource;

        public event EventHandler<TcpConnectionEventArgs> ConnectionClosed;

        internal ModbusServerTcpConnection(
            TcpClient tcpClient,
            IModbusServerNetwork serverNetwork,
            ILoggerFactory loggerFactory,
            SslServerAuthenticationOptions options)
        {
            connectionId = Interlocked.Increment(ref connectionIdSource);

            if (loggerFactory is null)
                throw new ArgumentNullException(nameof(loggerFactory));

            logger = loggerFactory.CreateLogger<ModbusServerTcpConnection>();
            this.tcpClient = tcpClient ?? throw new ArgumentNullException(nameof(tcpClient));
            this.serverNetwork = serverNetwork ?? throw new ArgumentNullException(nameof(serverNetwork));
            this.options = options;
        }

        /// <summary>
        /// Initializes the connection.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <remarks>This is broken out because we can't do async in the constructor, and we want to report errors back to the caller.</remarks>
        internal async Task IntializeAsync(CancellationToken cancellationToken)
        {
            var localStream = tcpClient.GetStream();

            if (options == null)
            {
                stream = new ModbusStream(localStream);
            }
            else
            {
                var sslStream = new SslStream(localStream, false);

                await sslStream.AuthenticateAsServerAsync(options, cancellationToken);

                stream = new ModbusStream(sslStream);
            }

            listenTask = Task.Run(() => ListenAsync(cancellationTokenSource.Token));
        }

        private async Task ListenAsync(CancellationToken cancellationToken)
        {
            try
            {

                if (stream == null)
                    throw new InvalidOperationException("You must call " + nameof(IntializeAsync) + " first.");

                while (!cancellationToken.IsCancellationRequested)
                {
                    var requestMessage = await stream.ReadIPMessageAsync(cancellationToken);

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
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occured in ListenAsync.");
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
