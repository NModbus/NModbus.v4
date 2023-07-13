using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Sockets;

namespace NModbus.Transport.Tcp.ConnectionStrategies
{
    public class SingletonTcpClientConnectionStrategy : ITcpClientConnectionStrategy
    {
        private readonly IPEndPoint endpoint;
        private readonly ILoggerFactory loggerFactory;
        private readonly Action<TcpClient> config;
        private TcpClient tcpClient;

        public SingletonTcpClientConnectionStrategy(IPEndPoint endpoint, ILoggerFactory loggerFactory, Action<TcpClient> config = null)
        {
            this.endpoint = endpoint ?? throw new ArgumentNullException(nameof(endpoint));
            this.loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
            this.config = config;
        }

        public async Task<ITcpClientRequestContainer> GetTcpClientAsync(CancellationToken cancellationToken)
        {
            if (tcpClient == null)
            {
                tcpClient = new TcpClient();

                if (config != null)
                {
                    config(tcpClient);
                }

                await tcpClient.ConnectAsync(endpoint, cancellationToken);
            }

            return new SingletonTcpClientRequestContainer(tcpClient);
        }

        public ValueTask DisposeAsync()
        {
            tcpClient?.Dispose();

            return ValueTask.CompletedTask;
        }
    }
}
