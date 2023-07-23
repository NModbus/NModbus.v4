using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Sockets;

namespace NModbus.Transport.Tcp.ConnectionStrategies
{
    public class SingletonTcpClientConnectionStrategy : ITcpClientConnectionStrategy
    {

        private readonly IPAddress ipAddress;
        private readonly int port;
        private readonly ILoggerFactory loggerFactory;
        private readonly Action<TcpClient> config;
        private TcpClient tcpClient;

        public SingletonTcpClientConnectionStrategy(IPAddress ipAddress, int port, ILoggerFactory loggerFactory, Action<TcpClient> config = null)
        {
            this.ipAddress = ipAddress ?? throw new ArgumentNullException(nameof(ipAddress));
            this.port = port;
            this.loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
            this.config = config;
        }

        public async Task<ITcpClientRequestContainer> GetTcpClientAsync(CancellationToken cancellationToken)
        {
            if (tcpClient == null)
            {
                tcpClient = new TcpClient();

                config?.Invoke(tcpClient);


                await tcpClient.ConnectAsync(ipAddress, port);
            }

            return new SingletonTcpClientRequestContainer(tcpClient);
        }

        public ValueTask DisposeAsync()
        {
            tcpClient?.Dispose();

            return default;
        }
    }
}
