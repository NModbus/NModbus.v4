using System.Net;
using System.Net.Security;
using System.Net.Sockets;

namespace NModbus.Transport.Tcp.ConnectionStrategies
{
    public class TcpClientFactory : ITcpClientFactory
    {
        private readonly IPAddress ipAddress;
        private readonly int port;
        private readonly SslClientAuthenticationOptions options;
        private readonly Action<TcpClient> config;

        public TcpClientFactory(
            IPAddress ipAddress,
            int port = ModbusTcpPorts.Insecure,
            Action<TcpClient> config = null,
            SslClientAuthenticationOptions options = null)
        {
            this.ipAddress = ipAddress ?? throw new ArgumentNullException(nameof(ipAddress));
            this.port = port;
            this.options = options;
            this.config = config;
        }

        public async Task<TcpClientWrapper> CreateAndConnectAsync(CancellationToken cancellationToken)
        {
            var tcpClient = new TcpClient();

            config?.Invoke(tcpClient);

            await tcpClient.ConnectAsync(this.ipAddress, this.port);

            if (options != null)
            {
                var sslStream = new SslStream(
                    tcpClient.GetStream(),
                    false);

                await sslStream.AuthenticateAsClientAsync(options, cancellationToken);

                return new TcpClientWrapper(tcpClient, sslStream);

            }
            else
            {
                return new TcpClientWrapper(tcpClient, tcpClient.GetStream());
            }
        }
    }

    public class TcpClientWrapper : IDisposable
    {
        private readonly TcpClient tcpClient;

        public TcpClientWrapper(TcpClient tcpClient, Stream stream)
        {
            this.tcpClient = tcpClient ?? throw new ArgumentNullException(nameof(tcpClient));
            Stream = stream ?? throw new ArgumentNullException(nameof(stream));
        }

        public Stream Stream { get; }

        public void Dispose()
        {
            Stream.Dispose();
            tcpClient.Dispose();
        }
    }

}
