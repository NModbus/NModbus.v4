namespace NModbus.Interfaces
{
    public interface IModbusTransport
    {
        Task SendAsync(byte unitNumber, ProtocolDataUnit data, CancellationToken cancellationToken = default);

        Task<ProtocolDataUnit> SendAndReceiveAsync(byte unitNumber, ProtocolDataUnit data, CancellationToken cancellationToken = default);
    }
}
