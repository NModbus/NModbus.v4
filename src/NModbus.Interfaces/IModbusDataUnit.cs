namespace NModbus.Interfaces
{
    /// <summary>
    /// There isn't a term in the Modbus spec that refers to the combination of a Unit Identifier with a Protocol Data Unit.
    /// An Application Data Unit can contain this and more, but is transport implementation specific. Hence - the term "Modbus Data Unit."
    /// </summary>
    /// <remarks>The term "ModbusMessage" was used initially, but this clashed with the concept of request / response messages.</remarks>
    public interface IModbusDataUnit
    {
        /// <summary>
        /// The Protocol Data Unit.
        /// </summary>
        ProtocolDataUnit ProtocolDataUnit { get; }

        /// <summary>
        /// The Unit Identifier.
        /// </summary>
        byte UnitIdentifier { get; }
    }
}