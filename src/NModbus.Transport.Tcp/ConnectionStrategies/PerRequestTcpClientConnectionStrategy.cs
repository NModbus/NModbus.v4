using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Sockets;

namespace NModbus.Transport.Tcp.ConnectionStrategies
{
    public class PerRequestTcpClientConnectionStrategy : ITcpClientConnectionStrategy
    {
        private readonly IPEndPoint endpoint;
        private readonly ILoggerFactory loggerFactory;
        private readonly Action<TcpClient> config;

        public PerRequestTcpClientConnectionStrategy(
            IPEndPoint endpoint, 
            ILoggerFactory loggerFactory, 
            Action<TcpClient> config = null)
        {
            this.endpoint = endpoint ?? throw new ArgumentNullException(nameof(endpoint));
            this.loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
            this.config = config;
        }

        public async Task<ITcpClientRequestContainer> GetTcpClientAsync(CancellationToken cancellationToken)
        {
            var tcpClient = new TcpClient();

            if (config != null)
            {
                config(tcpClient);
            }

            await tcpClient.ConnectAsync(endpoint, cancellationToken);

            return new PerRequestTcpClientRequestContainer(tcpClient);
        }

        public ValueTask DisposeAsync()
        {
            return ValueTask.CompletedTask;
        }
    }
}
