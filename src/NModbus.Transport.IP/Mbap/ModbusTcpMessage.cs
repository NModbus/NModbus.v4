using NModbus.Interfaces;

namespace NModbus.Transport.IP.Mbap
{
    internal class ModbusTcpMessage : IModbusDataUnit
    {
        public ModbusTcpMessage(MbapHeader header, ProtocolDataUnit protocolDataUnit)
        {
            Header = header ?? throw new ArgumentNullException(nameof(header));
            ProtocolDataUnit = protocolDataUnit ?? throw new ArgumentNullException(nameof(protocolDataUnit));
        }

        public MbapHeader Header { get; }

        public ProtocolDataUnit ProtocolDataUnit { get; }

        public byte UnitIdentifier => Header.UnitIdentifier;
    }
}
