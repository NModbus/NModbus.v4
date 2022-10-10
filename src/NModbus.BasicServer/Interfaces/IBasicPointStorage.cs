namespace NModbus.BasicServer.Interfaces
{
    public interface IBasicPointStorage<T>
    {
        Task<T> ReadPointAsync(ushort address);

        Task WritePointAsync(ushort address, T value);
    }
}
