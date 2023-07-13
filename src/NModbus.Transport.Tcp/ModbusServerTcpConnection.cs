using Microsoft.Extensions.Logging;
using NModbus.Interfaces;
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
        private readonly NetworkStream stream;
        private readonly CancellationTokenSource cancellationTokenSource = new();
        private readonly Task listenTask;
        private readonly IModbusClientTransport clientTransport;
        private readonly ILogger logger;
        private readonly int connectionId;

        private static int connectionIdSource;

        public event EventHandler<TcpConnectionEventArgs> ConnectionClosed;

        internal ModbusServerTcpConnection(
            TcpClient tcpClient,
            IModbusServerNetwork serverNetwork,
            ILoggerFactory loggerFactory)
        {
            connectionId = Interlocked.Increment(ref connectionIdSource);

            if (loggerFactory is null)
                throw new ArgumentNullException(nameof(loggerFactory));

            logger = loggerFactory.CreateLogger<ModbusServerTcpConnection>();
            this.tcpClient = tcpClient ?? throw new ArgumentNullException(nameof(tcpClient));
            clientTransport = new ModbusTcpClientTransport(new SimpleTcpClientLifetime(tcpClient.GetStream()), loggerFactory);
            this.serverNetwork = serverNetwork;
            stream = tcpClient.GetStream();
            listenTask = Task.Run(() => ListenAsync(cancellationTokenSource.Token));
        }

        private async Task ListenAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var message = await stream.ReadTcpMessage(cancellationToken);

                if (message == null)
                {
                    logger.LogInformation("{ConnectionId} closed after receiving 0 bytes.", connectionId);
                    OnConnectionClosed();
                    return;
                }

                logger.LogInformation("{ConnectionId} ModbusServerTcpConnection received ADU for unit {UnitIdentifier} with PDU FunctionCode {FunctionCode}.",
                    connectionId,
                    message.UnitIdentifier,
                    message.ProtocolDataUnit.FunctionCode);

                await serverNetwork.ProcessRequestAsync(message, clientTransport, cancellationToken);
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
