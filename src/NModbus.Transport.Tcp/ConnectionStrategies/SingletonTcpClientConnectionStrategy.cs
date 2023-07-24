using Microsoft.Extensions.Logging;

namespace NModbus.Transport.Tcp.ConnectionStrategies
{
    public class SingletonTcpClientConnectionStrategy : ITcpClientConnectionStrategy
    {
        private readonly ITcpClientFactory tcpClientFactory;
        private TcpClientWrapper tcpClientWrapper;

        public SingletonTcpClientConnectionStrategy(ITcpClientFactory tcpClientFactory, ILoggerFactory loggerFactory)
        {
            this.tcpClientFactory = tcpClientFactory ?? throw new ArgumentNullException(nameof(tcpClientFactory));
            if (loggerFactory == null) throw new ArgumentNullException(nameof(loggerFactory));
        }

        public async Task<ITcpClientRequestContainer> GetTcpClientAsync(CancellationToken cancellationToken)
        {
            tcpClientWrapper ??= await tcpClientFactory.CreateAndConnectAsync(cancellationToken);

            return new SingletonTcpClientRequestContainer(tcpClientWrapper);
        }

        public ValueTask DisposeAsync()
        {
            tcpClientWrapper?.Dispose();

            return default;
        }
    }
}
