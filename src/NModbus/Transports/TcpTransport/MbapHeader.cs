namespace NModbus.Transports.TcpTransport
{
    public class MbapHeader
    {
        public ushort TransactionIdentifier { get; set; }

        public ushort ProtocolIdentifier { get; set; }

        public ushort Length { get; set; }

        public byte UnitIdentifier { get; set; }
    }
}
