using Microsoft.Extensions.Logging;
using NModbus.Interfaces;
using System.Collections.Concurrent;
using System.Net.Sockets;

namespace NModbus.Transport.Tcp
{
    public class ModbusTcpServerNetworkTransport : IModbusServerNetworkTransport
    {
        private readonly TcpListener tcpListener;
        private readonly IModbusServerNetwork serverNetwork;
        private readonly ILoggerFactory loggerFactory;
        private readonly ILogger<ModbusTcpServerNetworkTransport> logger;
        private readonly ConcurrentDictionary<string, ModbusServerTcpConnection> connections = new();
        private readonly CancellationTokenSource cancellationTokenSource = new();
        private readonly Task listenTask;

        public ModbusTcpServerNetworkTransport(
            TcpListener tcpListener,
            IModbusServerNetwork serverNetwork,
            ILoggerFactory loggerFactory)
        {
            this.tcpListener = tcpListener ?? throw new ArgumentNullException(nameof(tcpListener));
            this.serverNetwork = serverNetwork ?? throw new ArgumentNullException(nameof(serverNetwork));
            this.loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
            logger = loggerFactory.CreateLogger<ModbusTcpServerNetworkTransport>();

            listenTask = Task.Run(() => ListenAsync(cancellationTokenSource.Token));
        }

        private async Task ListenAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting ModbusTcpServerNetworkTransport");

            tcpListener.Start();

            using (cancellationToken.Register(() => tcpListener?.Stop()))
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    try
                    {
                        var tcpClient = await tcpListener.AcceptTcpClientAsync()
                        .ConfigureAwait(false);

                        var endpoint = tcpClient.Client.RemoteEndPoint.ToString();

                        logger.LogInformation("Accepted a client from {Endpoint}", endpoint);

                        var serverConnection = new ModbusServerTcpConnection(tcpClient, serverNetwork, loggerFactory);

                        if (!connections.TryAdd(endpoint, serverConnection))
                            logger.LogWarning("Unable to add TCP server connection for '{Endpoint}'.", endpoint);

                        serverConnection.ConnectionClosed += ServerConnection_ConnectionClosed;
                    }
                    catch (SocketException ex) when (cancellationToken.IsCancellationRequested)
                    {
                        logger.LogTrace(ex, $"Swallowing {nameof(IOException)} in {nameof(ModbusTcpServerNetworkTransport)}.{nameof(ListenAsync)}");
                    }
                }
            }
        }

        private void ServerConnection_ConnectionClosed(object sender, TcpConnectionEventArgs e)
        {
            if (!connections.TryRemove(e.Endpoint, out _))
            {
                logger.LogWarning("Unable to remove '{Endpoint}' as it does not exist in the connections dictionary.", e.Endpoint);
            }
            else
            {
                logger.LogInformation("Connection from '{Endpoint}' has been removed.", e.Endpoint);
            }
        }

        public async ValueTask DisposeAsync()
        {
            cancellationTokenSource.Cancel();

            try
            {
                await listenTask;
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Error disposing {Object}", nameof(ModbusTcpServerNetworkTransport));
            }
        }
    }
}
