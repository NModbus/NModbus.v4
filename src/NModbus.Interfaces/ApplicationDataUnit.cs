namespace NModbus.Interfaces
{
    public class ApplicationDataUnit
    {
        public ApplicationDataUnit(byte unitNumber, ProtocolDataUnit protocolDataUnit)
        {
            UnitNumber = unitNumber;
            ProtocolDataUnit = protocolDataUnit ?? throw new ArgumentNullException(nameof(protocolDataUnit));
        }

        public ApplicationDataUnit(byte[] buffer)
        {
            UnitNumber = buffer[0];
            ProtocolDataUnit = new ProtocolDataUnit(buffer);
        }

        public byte UnitNumber { get; }

        public ProtocolDataUnit ProtocolDataUnit { get; }
    }
}
