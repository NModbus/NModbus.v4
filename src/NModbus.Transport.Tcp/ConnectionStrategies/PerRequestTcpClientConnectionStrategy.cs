using Microsoft.Extensions.Logging;
using System.Net.Sockets;

namespace NModbus.Transport.Tcp.ConnectionStrategies
{
    public class PerRequestTcpClientConnectionStrategy : ITcpClientConnectionStrategy
    {
        private readonly ITcpClientFactory tcpClientFactory;
        private readonly ILoggerFactory loggerFactory;

        public PerRequestTcpClientConnectionStrategy(ITcpClientFactory tcpClientFactory,
            ILoggerFactory loggerFactory)
        {
            this.tcpClientFactory = tcpClientFactory;
            this.loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        public async Task<ITcpClientRequestContainer> GetTcpClientAsync(CancellationToken cancellationToken)
        {
            var tcpClientWrapper = await tcpClientFactory.CreateAndConnectAsync(cancellationToken);

            return new PerRequestTcpClientRequestContainer(tcpClientWrapper);
        }

        public ValueTask DisposeAsync()
        {
            return default;
        }
    }
}
