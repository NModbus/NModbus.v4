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
        /// <param name="unitIdentifier"></param>
        /// <returns>True if the server was removed, false otherwise.</returns>
        bool TryRemoveServer(byte unitIdentifier);

        /// <summary>
        /// Processes a request from a Modbus Server network.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task ProcessRequestAsync(IModbusMessage requestMessage, IModbusClientTransport clientTransport, CancellationToken cancellationToken = default);
    }
}
