namespace NModbus.Transport.IP
{
    /// <summary>
    /// Represents a methodology of creating and or maintaining TCP Client connections.
    /// </summary>
    public interface IConnectionStrategy : IAsyncDisposable
    {
        /// <summary>
        /// Gets an object (that references a TcpClient) that has a lifetime of a single request and optionally a response.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IPerRequestStreamContainer> GetStreamContainer(CancellationToken cancellationToken);
    }
}
