namespace NModbus.Interfaces
{
    public interface IModbusServerNetwork
    {
        void AddServer(IModbusServer server);

        void RemoveServer(IModbusServer server);

        Task ListenAsync(CancellationToken cancellationToken);
    }
}
