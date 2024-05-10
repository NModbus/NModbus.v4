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
        private readonly TcpClient _tcpClient;
        private readonly IModbusServerNetwork _serverNetwork;
        private readonly SslServerAuthenticationOptions _options;
        private readonly CancellationTokenSource _cancellationTokenSource = new();
        private Task _listenTask;
        private readonly ILogger _logger;
        private readonly int _connectionId;

        private IModbusStream _stream;
        private static int _connectionIdSource;

        public event EventHandler<TcpConnectionEventArgs> ConnectionClosed;

        internal ModbusServerTcpConnection(
            TcpClient tcpClient,
            IModbusServerNetwork serverNetwork,
            ILoggerFactory loggerFactory,
            SslServerAuthenticationOptions options)
        {
            _connectionId = Interlocked.Increment(ref _connectionIdSource);

            if (loggerFactory is null)
                throw new ArgumentNullException(nameof(loggerFactory));

            _logger = loggerFactory.CreateLogger<ModbusServerTcpConnection>();
            _tcpClient = tcpClient ?? throw new ArgumentNullException(nameof(tcpClient));
            _serverNetwork = serverNetwork ?? throw new ArgumentNullException(nameof(serverNetwork));
            _options = options;
        }

        /// <summary>
        /// Initializes the connection.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <remarks>This is broken out because we can't do async in the constructor, and we want to report errors back to the caller.</remarks>
        internal async Task IntializeAsync(CancellationToken cancellationToken)
        {
            var localStream = _tcpClient.GetStream();

            if (_options == null)
            {
                _stream = new ModbusStream(localStream);
            }
            else
            {
                var sslStream = new SslStream(localStream, false);

                await sslStream.AuthenticateAsServerAsync(_options, cancellationToken);

                _stream = new ModbusStream(sslStream);
            }

            _listenTask = Task.Run(() => ListenAsync(_cancellationTokenSource.Token));
        }

        private async Task ListenAsync(CancellationToken cancellationToken)
        {
            try
            {

                if (_stream == null)
                    throw new InvalidOperationException("You must call " + nameof(IntializeAsync) + " first.");

                while (!cancellationToken.IsCancellationRequested)
                {
                    var requestMessage = await _stream.ReadIpMessageAsync(cancellationToken);

                    if (requestMessage == null)
                    {
                        _logger.LogInformation("{ConnectionId} closed after receiving 0 bytes.", _connectionId);
                        OnConnectionClosed();
                        return;
                    }

                    _logger.LogInformation("{ConnectionId} ModbusServerTcpConnection received ADU for unit {UnitIdentifier} with PDU FunctionCode {FunctionCode}.",
                        _connectionId,
                        requestMessage.UnitIdentifier,
                        requestMessage.ProtocolDataUnit.FunctionCode);

                    await using (var backchannelTransport = new BackchannelTcpClientTransport(_stream, requestMessage.Header.TransactionIdentifier))
                    {
                        await _serverNetwork.ProcessRequestAsync(requestMessage, backchannelTransport, cancellationToken);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured in ListenAsync.");
            }
        }

        protected void OnConnectionClosed()
        {
            ConnectionClosed?.Invoke(this, new TcpConnectionEventArgs(_tcpClient.Client.RemoteEndPoint.ToString()));
        }

        public async ValueTask DisposeAsync()
        {
            _cancellationTokenSource.Dispose();

            await _listenTask;
        }
    }
}
