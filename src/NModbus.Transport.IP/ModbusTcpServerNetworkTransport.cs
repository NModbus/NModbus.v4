using Microsoft.Extensions.Logging;
using NModbus.Interfaces;
using System.Collections.Concurrent;
using System.Net.Security;
using System.Net.Sockets;

namespace NModbus.Transport.IP
{
    public class ModbusTcpServerNetworkTransport : IModbusServerNetworkTransport
    {
        private readonly TcpListener _tcpListener;
        private readonly IModbusServerNetwork _serverNetwork;
        private readonly ILoggerFactory _loggerFactory;
        private readonly ILogger<ModbusTcpServerNetworkTransport> _logger;
        private readonly SslServerAuthenticationOptions _options;
        private readonly ConcurrentDictionary<string, ModbusServerTcpConnection> _connections = new();
        private readonly CancellationTokenSource _cancellationTokenSource = new();
        private readonly Task _listenTask;

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
            _tcpListener = tcpListener ?? throw new ArgumentNullException(nameof(tcpListener));
            _serverNetwork = serverNetwork ?? throw new ArgumentNullException(nameof(serverNetwork));
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
            _options = options;
            _logger = loggerFactory.CreateLogger<ModbusTcpServerNetworkTransport>();

            _listenTask = Task.Run(() => ListenAsync(_cancellationTokenSource.Token));
        }


        private async Task ListenAsync(CancellationToken cancellationToken)
        {
            if (_options == null)
            {
                _logger.LogInformation("Starting " + nameof(ModbusTcpServerNetworkTransport) + " with insecure endpoint on {Endpoint}", _tcpListener.LocalEndpoint);
            }
            else
            {
                _logger.LogInformation("Starting " + nameof(ModbusTcpServerNetworkTransport) + " with secure endpoint on {Endpoint}", _tcpListener.LocalEndpoint);
            }

            _tcpListener.Start();

            using (cancellationToken.Register(() => _tcpListener?.Stop()))
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    var tcpClient = await _tcpListener.AcceptTcpClientAsync()
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

                _logger.LogInformation("Accepted a client from {Endpoint}", endpoint);

                var serverConnection = new ModbusServerTcpConnection(tcpClient, _serverNetwork, _loggerFactory, _options);

                await serverConnection.IntializeAsync(cancellationToken)
                    .ConfigureAwait(false);

                if (!_connections.TryAdd(endpoint, serverConnection))
                    _logger.LogWarning("Unable to add TCP server connection for '{Endpoint}'.", endpoint);

                serverConnection.ConnectionClosed += ServerConnection_ConnectionClosed;
            }
            catch (SocketException ex) when (cancellationToken.IsCancellationRequested)
            {
                _logger.LogWarning(ex, $"Swallowing {nameof(IOException)} in {nameof(ModbusTcpServerNetworkTransport)}.{nameof(ListenAsync)}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Problem in {nameof(StartClientProcessing)}");
            }
        }

        private void ServerConnection_ConnectionClosed(object sender, TcpConnectionEventArgs e)
        {
            if (!_connections.TryRemove(e.Endpoint, out _))
            {
                _logger.LogWarning("Unable to remove '{Endpoint}' as it does not exist in the connections dictionary.", e.Endpoint);
            }
            else
            {
                _logger.LogInformation("Connection from '{Endpoint}' has been removed.", e.Endpoint);
            }
        }

        public async ValueTask DisposeAsync()
        {
            _cancellationTokenSource.Cancel();

            try
            {
                await _listenTask;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error disposing {Object}", nameof(ModbusTcpServerNetworkTransport));
            }
        }
    }
}
