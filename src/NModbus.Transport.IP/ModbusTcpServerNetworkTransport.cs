using Microsoft.Extensions.Logging;
using NModbus.Interfaces;
using System.Collections.Concurrent;
using System.Net.Security;
using System.Net.Sockets;

namespace NModbus.Transport.IP
{
    public class ModbusTcpServerNetworkTransport : IModbusServerNetworkTransport
    {
        private readonly TcpListener tcpListener;
        private readonly IModbusServerNetwork serverNetwork;
        private readonly ILoggerFactory loggerFactory;
        private readonly ILogger<ModbusTcpServerNetworkTransport> logger;
        private readonly SslServerAuthenticationOptions options;
        private readonly ConcurrentDictionary<string, ModbusServerTcpConnection> connections = new();
        private readonly CancellationTokenSource cancellationTokenSource = new();
        private readonly Task listenTask;

        /// <summary>
        /// Creates an in stance of <see cref="ModbusTcpServerNetworkTransport"/>.
        /// </summary>
        /// <param name="tcpListener">A configured <see cref="TcpListener"/> that will listen for incoming connections.</param>
        /// <param name="serverNetwork">The network of Modbus servers.</param>
        /// <param name="loggerFactory"></param>
        /// <param name="options">Specify a value to enable Tls (Modbus Security Spec).</param>
        /// <exception cref="ArgumentNullException"></exception>
        public ModbusTcpServerNetworkTransport(
            TcpListener tcpListener,
            IModbusServerNetwork serverNetwork,
            ILoggerFactory loggerFactory,
            SslServerAuthenticationOptions options = null)
        {
            this.tcpListener = tcpListener ?? throw new ArgumentNullException(nameof(tcpListener));
            this.serverNetwork = serverNetwork ?? throw new ArgumentNullException(nameof(serverNetwork));
            this.loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
            this.options = options;
            logger = loggerFactory.CreateLogger<ModbusTcpServerNetworkTransport>();

            listenTask = Task.Run(() => ListenAsync(cancellationTokenSource.Token));
        }


        private async Task ListenAsync(CancellationToken cancellationToken)
        {
            if (options == null)
            {
                logger.LogInformation("Starting " + nameof(ModbusTcpServerNetworkTransport) + " with insecure endpoint on {Endpoint}", tcpListener.LocalEndpoint);
            }
            else
            {
                logger.LogInformation("Starting " + nameof(ModbusTcpServerNetworkTransport) + " with secure endpoint on {Endpoint}", tcpListener.LocalEndpoint);
            }

            tcpListener.Start();

            using (cancellationToken.Register(() => tcpListener?.Stop()))
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    var tcpClient = await tcpListener.AcceptTcpClientAsync()
                        .ConfigureAwait(false);

                    await StartClientProcessing(tcpClient, cancellationToken);
                }
            }
        }

        private async Task StartClientProcessing(TcpClient tcpClient, CancellationToken cancellationToken)
        {
            try
            {
                var endpoint = tcpClient.Client.RemoteEndPoint.ToString();

                logger.LogInformation("Accepted a client from {Endpoint}", endpoint);

                var serverConnection = new ModbusServerTcpConnection(tcpClient, serverNetwork, loggerFactory, options);

                await serverConnection.IntializeAsync(cancellationToken)
                    .ConfigureAwait(false);

                if (!connections.TryAdd(endpoint, serverConnection))
                    logger.LogWarning("Unable to add TCP server connection for '{Endpoint}'.", endpoint);

                serverConnection.ConnectionClosed += ServerConnection_ConnectionClosed;
            }
            catch (SocketException ex) when (cancellationToken.IsCancellationRequested)
            {
                logger.LogWarning(ex, $"Swallowing {nameof(IOException)} in {nameof(ModbusTcpServerNetworkTransport)}.{nameof(ListenAsync)}");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Problem in {nameof(StartClientProcessing)}");
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
