namespace NModbus.Transport.Tcp
{
    /// <summary>
    /// Represents a methodology of creating and or maintaining TCP Client connections.
    /// </summary>
    public interface ITcpClientConnectionStrategy : IAsyncDisposable
    {
        /// <summary>
        /// Gets an object (that references a TcpClient) that has a lifetime of a single request and optionally a response.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ITcpClientRequestContainer> GetTcpClientAsync(CancellationToken cancellationToken);
    }
}
