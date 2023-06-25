using NModbus.Interfaces;

namespace NModbus.BasicServer.Interfaces
{
    /// <summary>
    /// This is how the server device accesses storage.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="IApplicationPointStorage{T}"/>
    public interface IDevicePointStorage<T>
    {
        /// <summary>
        /// Reads the specified number of points from the device.
        /// </summary>
        /// <param name="startingAddress"></param>
        /// <param name="numberOfPoints"></param>
        /// <returns></returns>
        /// <exception cref="ModbusServerException"/>
        T[] ReadPoints(ushort startingAddress, ushort numberOfPoints);

        /// <summary>
        /// Writes the specified number of points from the device.
        /// </summary>
        /// <param name="startingAddress"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ModbusServerException"/>
        void WritePoints(ushort startingAddress, T[] value);
    }
}
