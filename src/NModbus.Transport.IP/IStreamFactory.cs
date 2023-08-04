using NModbus.Interfaces;

namespace NModbus.Transport.IP
{
    /// <summary>
    /// Responsible for creating streams.
    /// </summary>
    public interface IStreamFactory
    {
        /// <summary>
        /// Create a connection and open that connection.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IModbusStream> CreateAndConnectAsync(CancellationToken cancellationToken);
    }
}