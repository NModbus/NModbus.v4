namespace NModbus.Interfaces
{
    /// <summary>
    /// Processes requests and return an appropriate response (or error message)
    /// </summary>
    public interface IModbusServer
    {
        /// <summary>
        /// Gets the unit number for this server device.
        /// </summary>
        byte UnitIdentifier { get; }

        /// <summary>
        /// Process the request.
        /// </summary>
        /// <returns></returns>
        Task<ProtocolDataUnit> ProcessRequestAsync(ProtocolDataUnit protocolDataUnit, CancellationToken cancellationToken = default);
    }
}
