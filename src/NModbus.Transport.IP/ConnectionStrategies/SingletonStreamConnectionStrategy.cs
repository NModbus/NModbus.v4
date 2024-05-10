using Microsoft.Extensions.Logging;
using NModbus.Interfaces;

namespace NModbus.Transport.IP.ConnectionStrategies
{
    public class SingletonStreamConnectionStrategy : IConnectionStrategy
    {
        private readonly IStreamFactory _tcpClientFactory;
        private IModbusStream _stream;

        public SingletonStreamConnectionStrategy(IStreamFactory streamFactory, ILoggerFactory loggerFactory)
        {
            _tcpClientFactory = streamFactory ?? throw new ArgumentNullException(nameof(streamFactory));
            if (loggerFactory == null) throw new ArgumentNullException(nameof(loggerFactory));
        }

        public async Task<IPerRequestStreamContainer> GetStreamContainer(CancellationToken cancellationToken)
        {
            _stream ??= await _tcpClientFactory.CreateAndConnectAsync(cancellationToken);

            return new SingletonStreamPerRequestContainer(_stream);
        }

        public async ValueTask DisposeAsync()
        {
            GC.SuppressFinalize(this);

            if (_stream != null)
            {
                await _stream.DisposeAsync();
            }
        }
    }
}
