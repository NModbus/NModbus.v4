using Microsoft.Extensions.Logging;
using NModbus.Transport.Tcp;

namespace NModbus.Transport.IP.ConnectionStrategies
{
    public class SingletonTcpClientConnectionStrategy : ITcpConnectionStrategy
    {
        private readonly IStreamFactory tcpClientFactory;
        private StreamWrapper tcpClientWrapper;

        public SingletonTcpClientConnectionStrategy(IStreamFactory streamFactory, ILoggerFactory loggerFactory)
        {
            tcpClientFactory = streamFactory ?? throw new ArgumentNullException(nameof(streamFactory));
            if (loggerFactory == null) throw new ArgumentNullException(nameof(loggerFactory));
        }

        public async Task<IPerRequestStreamContainer> GetTcpClientAsync(CancellationToken cancellationToken)
        {
            tcpClientWrapper ??= await tcpClientFactory.CreateAndConnectAsync(cancellationToken);

            return new SingletonStreamPerRequestContainer(tcpClientWrapper);
        }

        public ValueTask DisposeAsync()
        {
            GC.SuppressFinalize(this);

            tcpClientWrapper?.Dispose();

            return default;
        }
    }
}
