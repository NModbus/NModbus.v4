using System.Net.Sockets;

namespace NModbus.Transports.TcpTransport
{
    public class SimpleTcpClientLifetime : ITcpClientLifetime
    {
        private readonly Stream stream;

        public SimpleTcpClientLifetime(Stream stream)
        {
            this.stream = stream ?? throw new ArgumentNullException(nameof(stream));
        }

        public Task<ITcpClientContainer> GetTcpClientAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult((ITcpClientContainer)new SimpleTcpClientContainer(stream));
        }

        public ValueTask DisposeAsync()
        {
            return ValueTask.CompletedTask;
        }
    }
}
