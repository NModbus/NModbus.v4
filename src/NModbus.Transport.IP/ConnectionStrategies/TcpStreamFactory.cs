using NModbus.Transport.Tcp;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;

namespace NModbus.Transport.IP.ConnectionStrategies
{
    /// <summary>
    /// Responsible for creating a client.
    /// </summary>
    public class TcpStreamFactory : IStreamFactory
    {
        private readonly IPAddress ipAddress;
        private readonly int port;
        private readonly SslClientAuthenticationOptions options;
        private readonly Action<TcpClient> tcpClientConfig;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ipAddress">The <see cref="IPAddress"/> to connect to.</param>
        /// <param name="port">The port to connect to.</param>
        /// <param name="tcpClientConfig">Custom configuration action for <see cref="TcpClient"/>.</param>
        /// <param name="sslOptions">If non-null, an ssl connection will be created with the specified options.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public TcpStreamFactory(
            IPAddress ipAddress,
            int port = ModbusTcpPorts.Insecure,
            Action<TcpClient> tcpClientConfig = null,
            SslClientAuthenticationOptions sslOptions = null)
        {
            this.ipAddress = ipAddress ?? throw new ArgumentNullException(nameof(ipAddress));
            this.port = port;
            options = sslOptions;
            this.tcpClientConfig = tcpClientConfig;
        }

        /// <summary>
        /// Creates a connection and performs a connection.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <remarks>
        /// Optionally creates an <see cref="SslStream"/> if an <see cref="SslClientAuthenticationOptions"/> instance is provided in the constructor.
        /// </remarks>
        public async Task<StreamWrapper> CreateAndConnectAsync(CancellationToken cancellationToken)
        {
            var tcpClient = new TcpClient();

            tcpClientConfig?.Invoke(tcpClient);

            await tcpClient.ConnectAsync(ipAddress, port);

            if (options != null)
            {
                var sslStream = new SslStream(
                    tcpClient.GetStream(),
                    false);

                await sslStream.AuthenticateAsClientAsync(options, cancellationToken);

                return new StreamWrapper(sslStream, tcpClient);

            }
            else
            {
                return new StreamWrapper(tcpClient.GetStream(), tcpClient);
            }
        }
    }
}
