namespace NModbus.Transports.TcpTransport
{
    public class MbapHeader
    {
        public MbapHeader(ushort transactionIdentifier, ushort protocolIdentifier, ushort length, byte unitIdentifier)
        {
            TransactionIdentifier = transactionIdentifier;
            ProtocolIdentifier = protocolIdentifier;
            Length = length;
            UnitIdentifier = unitIdentifier;
        }

        public ushort TransactionIdentifier { get; }

        public ushort ProtocolIdentifier { get; }

        public ushort Length { get; }

        public byte UnitIdentifier { get; }
    }
}
