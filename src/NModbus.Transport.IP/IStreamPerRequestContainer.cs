using System.Net.Sockets;

namespace NModbus.Transport.Tcp
{
    /// <summary>
    /// This container will exist for the lifetime of a single TCP request and optional response.
    /// </summary>
    public interface IPerRequestStreamContainer : IAsyncDisposable
    {
        /// <summary>
        /// Gets the underlying stream
        /// </summary>
        Stream Stream { get; }
    }
}
