namespace NModbus.Transport.IP.Mbap
{
    internal record MbapHeader(ushort TransactionIdentifier, ushort ProtocolIdentifier, ushort Length, byte UnitIdentifier);
}
