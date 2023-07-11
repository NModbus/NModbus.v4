using System.Net.Sockets;

namespace NModbus.Transports.TcpTransport
{
    public interface ITcpClientContainer : IAsyncDisposable
    {
        Stream Stream { get; }
    }
}
