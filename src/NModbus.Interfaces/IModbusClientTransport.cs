namespace NModbus.Interfaces
{
    /// <summary>
    /// The transport for a Modbus client.
    /// </summary>
    public interface IModbusClientTransport : IAsyncDisposable
    {
        /// <summary>
        /// Send a message but don't want for a response.
        /// </summary>
        /// <param name="applicationDataUnit"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task SendAsync(ApplicationDataUnit applicationDataUnit, CancellationToken cancellationToken = default);

        /// <summary>
        /// Send a message and wait for a response.
        /// </summary>
        /// <param name="applicationDataUnit"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ApplicationDataUnit> SendAndReceiveAsync(ApplicationDataUnit applicationDataUnit, CancellationToken cancellationToken = default);
    }
}
