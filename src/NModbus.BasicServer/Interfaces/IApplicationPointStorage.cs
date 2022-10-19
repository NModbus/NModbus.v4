namespace NModbus.BasicServer.Interfaces
{
    /// <summary>
    /// This is storage from the application perspective.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="IDevicePointStorage{T}"/>
    public interface IApplicationPointStorage<T>
    {
        /// <summary>
        /// Invoked before <see cref="IDevicePointStorage{T}.ReadPointsAsync(ushort, ushort)"/>
        /// </summary>
        event EventHandler<DeviceReadArgs> BeforeDeviceRead;

        /// <summary>
        /// Invoked after <see cref="IDevicePointStorage{T}.ReadPointsAsync(ushort, ushort)"/>
        /// </summary>
        event EventHandler<DeviceReadArgs> AfterDeviceRead;

        /// <summary>
        /// Invoked before <see cref="IDevicePointStorage{T}.WritePointsAsync(ushort, T[])"/>
        /// </summary>
        event EventHandler<DeviceWriteArgs<T>> BeforeDeviceWrite;

        /// <summary>
        /// Invoked after <see cref="IDevicePointStorage{T}.WritePointsAsync(ushort, T[])"/>
        /// </summary>
        event EventHandler<DeviceWriteArgs<T>> AfterDeviceWrite;

        /// <summary>
        /// Gets or sets the value of a point. This does NOT invoke Before/After Read/Write events.
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        T this[ushort address] { get; set; }
    }

    public interface IPointStorage<T> : IDevicePointStorage<T>, IApplicationPointStorage<T>
    {
    }
}
