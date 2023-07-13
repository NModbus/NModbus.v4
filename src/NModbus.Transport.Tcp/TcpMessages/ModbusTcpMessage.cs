using NModbus.Interfaces;

namespace NModbus.Transport.Tcp.TcpMessages
{
    internal class ModbusTcpMessage : IModbusMessage
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
