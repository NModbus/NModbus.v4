using Microsoft.Extensions.Logging;
using NModbus.Interfaces;

namespace NModbus.Transport.IP.ConnectionStrategies
{
    public class SingletonStreamConnectionStrategy : IConnectionStrategy
    {
        private readonly IStreamFactory tcpClientFactory;
        private IModbusStream stream;

        public SingletonStreamConnectionStrategy(IStreamFactory streamFactory, ILoggerFactory loggerFactory)
        {
            tcpClientFactory = streamFactory ?? throw new ArgumentNullException(nameof(streamFactory));
            if (loggerFactory == null) throw new ArgumentNullException(nameof(loggerFactory));
        }

        public async Task<IPerRequestStreamContainer> GetStreamContainer(CancellationToken cancellationToken)
        {
            stream ??= await tcpClientFactory.CreateAndConnectAsync(cancellationToken);

            return new SingletonStreamPerRequestContainer(stream);
        }

        public async ValueTask DisposeAsync()
        {
            GC.SuppressFinalize(this);

            if (stream != null)
            {
                await stream.DisposeAsync();
            }
        }
    }
}
