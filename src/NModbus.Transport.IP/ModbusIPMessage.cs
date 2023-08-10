using NModbus.Interfaces;
using NModbus.Transport.IP.Mbap;

namespace NModbus.Transport.IP
{
    public class ModbusIPMessage : IModbusDataUnit
    {
        public ModbusIPMessage(MbapHeader header, ProtocolDataUnit protocolDataUnit)
        {
            Header = header ?? throw new ArgumentNullException(nameof(header));
            ProtocolDataUnit = protocolDataUnit ?? throw new ArgumentNullException(nameof(protocolDataUnit));
        }

        public MbapHeader Header { get; }

        public ProtocolDataUnit ProtocolDataUnit { get; }

        public byte UnitIdentifier => Header.UnitIdentifier;
    }
}
