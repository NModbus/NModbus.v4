using System.Net.Sockets;

namespace NModbus.Transport.Tcp.ConnectionStrategies
{
    internal class PerRequestTcpClientRequestContainer : ITcpClientRequestContainer
    {
        internal PerRequestTcpClientRequestContainer(TcpClient tcpClient)
        {
            TcpClient = tcpClient ?? throw new ArgumentNullException(nameof(tcpClient));
        }

        public TcpClient TcpClient { get; }

        public ValueTask DisposeAsync()
        {
            TcpClient.Dispose();
            return default;
        }
    }
}
