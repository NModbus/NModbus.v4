using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Sockets;

namespace NModbus.Transports.TcpTransport
{
    public class PersistentTcpClientLifetime : ITcpClientLifetime
    {
        private readonly IPEndPoint endpoint;
        private readonly ILoggerFactory loggerFactory;
        private TcpClient tcpClient;

        public PersistentTcpClientLifetime(IPEndPoint endpoint, ILoggerFactory loggerFactory)
        {
            this.endpoint = endpoint ?? throw new ArgumentNullException(nameof(endpoint));
            this.loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        public async Task<ITcpClientContainer> GetTcpClientAsync(CancellationToken cancellationToken)
        {
            if (tcpClient == null)
            {
                //TODO: Lock and double check null
                tcpClient = new TcpClient();

                await tcpClient.ConnectAsync(endpoint, cancellationToken);
            }

            return new SimpleTcpClientContainer(tcpClient.GetStream());
        }

        public ValueTask DisposeAsync()
        {
            tcpClient?.Dispose();

            return ValueTask.CompletedTask;
        }
    }
}
