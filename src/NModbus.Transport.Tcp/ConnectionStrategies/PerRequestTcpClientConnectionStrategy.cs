using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Sockets;

namespace NModbus.Transport.Tcp.ConnectionStrategies
{
    public class PerRequestTcpClientConnectionStrategy : ITcpClientConnectionStrategy
    {
        private readonly IPAddress ipAddress;
        private readonly int port;
        private readonly ILoggerFactory loggerFactory;
        private readonly Action<TcpClient> config;

        public PerRequestTcpClientConnectionStrategy(
            IPAddress ipAddress, 
            int port,
            ILoggerFactory loggerFactory, 
            Action<TcpClient> config = null)
        {
            this.ipAddress = ipAddress ?? throw new ArgumentNullException(nameof(ipAddress));
            this.port = port;
            this.loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
            this.config = config;
        }

        public async Task<ITcpClientRequestContainer> GetTcpClientAsync(CancellationToken cancellationToken)
        {
            var tcpClient = new TcpClient();

            config?.Invoke(tcpClient);

            await tcpClient.ConnectAsync(ipAddress, port);

            return new PerRequestTcpClientRequestContainer(tcpClient);
        }

        public ValueTask DisposeAsync()
        {
            return default;
        }
    }
}
