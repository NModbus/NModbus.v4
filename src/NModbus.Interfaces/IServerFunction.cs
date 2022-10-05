namespace NModbus.Interfaces
{
    public interface IServerFunction
    {
        byte FunctionCode { get; }

        Task<byte[]> ProcessAsync(byte[] data, CancellationToken cancellationToken);
    }
}
