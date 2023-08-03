using Microsoft.Extensions.Logging;
using NModbus.Transport.Tcp;

namespace NModbus.Transport.IP.ConnectionStrategies
{
    public class PerRequestTcpConnectionStrategy : ITcpConnectionStrategy
    {
        private readonly IStreamFactory streamFactory;
        private readonly ILoggerFactory loggerFactory;

        public PerRequestTcpConnectionStrategy(IStreamFactory tcpClientFactory,
            ILoggerFactory loggerFactory)
        {
            streamFactory = tcpClientFactory;
            this.loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        public async Task<IPerRequestStreamContainer> GetTcpClientAsync(CancellationToken cancellationToken)
        {
            var wrapper = await streamFactory.CreateAndConnectAsync(cancellationToken);

            return new StreamPerRequestContainer(wrapper);
        }

        public ValueTask DisposeAsync()
        {
            GC.SuppressFinalize(this);

            return default;
        }
    }
}
