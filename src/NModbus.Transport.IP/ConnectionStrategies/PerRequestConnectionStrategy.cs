using Microsoft.Extensions.Logging;
using NModbus.Interfaces;

namespace NModbus.Transport.IP.ConnectionStrategies
{
    public class PerRequestConnectionStrategy : IConnectionStrategy
    {
        private readonly IStreamFactory _streamFactory;
        private readonly ILoggerFactory _loggerFactory;

        public PerRequestConnectionStrategy(IStreamFactory tcpClientFactory,
            ILoggerFactory loggerFactory)
        {
            _streamFactory = tcpClientFactory;
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        public async Task<IPerRequestStreamContainer> GetStreamContainer(CancellationToken cancellationToken)
        {
            var wrapper = await _streamFactory.CreateAndConnectAsync(cancellationToken);

            return new StreamPerRequestContainer(wrapper);
        }

        public ValueTask DisposeAsync()
        {
            GC.SuppressFinalize(this);

            return default;
        }
    }
}
