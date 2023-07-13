namespace NModbus.Transport.Tcp.TcpMessages
{
    internal record MbapHeader(ushort TransactionIdentifier, ushort ProtocolIdentifier, ushort Length, byte UnitIdentifier);
}
