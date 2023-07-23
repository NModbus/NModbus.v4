using System.Net.Sockets;

namespace NModbus.Transport.Tcp.ConnectionStrategies
{
    internal class SingletonTcpClientRequestContainer : ITcpClientRequestContainer
    {
        public SingletonTcpClientRequestContainer(TcpClient tcpClient)
        {
            TcpClient = tcpClient ?? throw new ArgumentNullException(nameof(tcpClient));
        }

        public TcpClient TcpClient { get; }

        public ValueTask DisposeAsync()
        {
            return default;
        }
    }
}
