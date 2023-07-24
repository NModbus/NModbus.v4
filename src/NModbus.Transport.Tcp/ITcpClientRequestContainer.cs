using System.Net.Sockets;

namespace NModbus.Transport.Tcp
{
    /// <summary>
    /// This container will exist for the lifetime of a single TCP request and optional response.
    /// </summary>
    public interface ITcpClientRequestContainer : IAsyncDisposable
    {
        Stream Stream { get; }
    }
}
