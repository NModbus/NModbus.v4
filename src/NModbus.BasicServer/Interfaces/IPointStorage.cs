namespace NModbus.BasicServer.Interfaces
{
    public interface IPointStorage<T> : IDevicePointStorage<T>, IApplicationPointStorage<T>
    {
    }
}
