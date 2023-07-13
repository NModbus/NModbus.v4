namespace NModbus.Transport.Tcp
{
    internal record MbapHeader(ushort TransactionIdentifier, ushort ProtocolIdentifier, ushort Length, byte UnitIdentifier);
}
