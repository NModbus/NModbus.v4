namespace NModbus.Interfaces
{
    /// <summary>
    /// Processes requests and return an appropriate response (or error message)
    /// </summary>
    public interface IModbusServer
    {
        Task ListenAsync(CancellationToken cancellationToken = default);
    }
}
