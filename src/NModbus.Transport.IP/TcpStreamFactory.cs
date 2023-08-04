using NModbus.Interfaces;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;

namespace NModbus.Transport.IP
{
    /// <summary>
    /// Responsible for creating a client.
    /// </summary>
    public class TcpStreamFactory : IStreamFactory
    {
        private readonly IPEndPoint endPoint;
        private readonly Action<TcpClient> tcpClientConfig;
        private readonly SslClientAuthenticationOptions sslOptions;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="endPoint"></param>
        /// <param name="tcpClientConfig">Custom configuration action for <see cref="TcpClient"/>.</param>
        /// <param name="sslOptions">If non-null, an ssl connection will be created with the specified options.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public TcpStreamFactory(
            IPEndPoint endPoint,
            Action<TcpClient> tcpClientConfig = null,
            SslClientAuthenticationOptions sslOptions = null)
        {
            this.endPoint = endPoint ?? throw new ArgumentNullException(nameof(endPoint));
            this.tcpClientConfig = tcpClientConfig;
            this.sslOptions = sslOptions;
        }

        /// <summary>
        /// Creates a connection and performs a connection.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <remarks>
        /// Optionally creates an <see cref="SslStream"/> if an <see cref="SslClientAuthenticationOptions"/> instance is provided in the constructor.
        /// </remarks>
        public async Task<IModbusStream> CreateAndConnectAsync(CancellationToken cancellationToken)
        {
            var tcpClient = new TcpClient();

            tcpClientConfig?.Invoke(tcpClient);

            await tcpClient.ConnectAsync(endPoint.Address, endPoint.Port);

            if (sslOptions != null)
            {
                var sslStream = new SslStream(
                    tcpClient.GetStream(),
                    false);

                await sslStream.AuthenticateAsClientAsync(sslOptions, cancellationToken);

                return new TcpModbusStream(tcpClient, sslStream);

            }
            else
            {
                return new TcpModbusStream(tcpClient, tcpClient.GetStream());
            }
        }
    }
}
