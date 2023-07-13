namespace NModbus.Interfaces
{
    public interface IModbusMessage
    {
        ProtocolDataUnit ProtocolDataUnit { get; }
        byte UnitIdentifier { get; }
    }
}