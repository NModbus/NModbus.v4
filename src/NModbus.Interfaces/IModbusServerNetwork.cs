namespace NModbus.Interfaces
{
    /// <summary>
    /// A network of Modbus servers.
    /// </summary>
    public interface IModbusServerNetwork
    {
        /// <summary>
        /// Attempts to add a server to this network.
        /// </summary>
        /// <param name="server"></param>
        /// <returns>True if the server was added, false otherwise.</returns>
        bool TryAddServer(IModbusServer server);

        /// <summary>
        /// Attempts to remove the Modbus server from this network.
        /// </summary>
        /// <param name="unitNumber"></param>
        /// <returns>True if the server was removed, false otherwise.</returns>
        bool TryRemoveServer(byte unitNumber);

        /// <summary>
        /// Processes a request from a Modbus Server network.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task ProcessRequestAsync(ApplicationDataUnit applicationDataUnit, IModbusClientTransport clientTransport, CancellationToken cancellationToken = default);
    }
}
