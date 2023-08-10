namespace NModbus.Transport.IP.Mbap
{
    public record MbapHeader(ushort TransactionIdentifier, ushort ProtocolIdentifier, ushort Length, byte UnitIdentifier);
}
