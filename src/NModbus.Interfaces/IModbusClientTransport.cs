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
        /// <param name="message"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task SendAsync(IModbusMessage message, CancellationToken cancellationToken = default);

        /// <summary>
        /// Send a message and wait for a response.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IModbusMessage> SendAndReceiveAsync(IModbusMessage message, CancellationToken cancellationToken = default);
    }
}
