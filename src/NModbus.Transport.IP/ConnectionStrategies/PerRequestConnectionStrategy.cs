using Microsoft.Extensions.Logging;
using NModbus.Interfaces;

namespace NModbus.Transport.IP.ConnectionStrategies
{
    public class PerRequestConnectionStrategy : IConnectionStrategy
    {
        private readonly IStreamFactory streamFactory;
        private readonly ILoggerFactory loggerFactory;

        public PerRequestConnectionStrategy(IStreamFactory tcpClientFactory,
            ILoggerFactory loggerFactory)
        {
            streamFactory = tcpClientFactory;
            this.loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        public async Task<IPerRequestStreamContainer> GetStreamContainer(CancellationToken cancellationToken)
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
