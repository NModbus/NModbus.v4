using NModbus.BasicServer.Interfaces;
using NModbus.Interfaces;
using System.Collections.Concurrent;

namespace NModbus.BasicServer
{
    /// <summary>
    /// Stores points using a Dictionary rather than an array.
    /// </summary>
    /// <remarks>
    /// Rather than allocate a full array of <see cref="UInt16.MaxValue"/> values, we use a <see cref="ConcurrentDictionary{UInt16, TValue}"/> 
    /// to store only those addresses which have been set.
    /// </remarks>
    /// <typeparam name="T"></typeparam>
    public class SparsePointStorage<T> : IPointStorage<T>
    {
        private readonly ConcurrentDictionary<ushort, T> values = new ConcurrentDictionary<ushort, T>();

        public T this[ushort address] 
        { 
            get
            {
                values.TryGetValue(address, out var value);

                return value;
            }
            set
            {
                values[address] = value;
            }
        }

        public event EventHandler<DeviceReadArgs> BeforeDeviceRead;
        public event EventHandler<DeviceReadArgs> AfterDeviceRead;
        public event EventHandler<DeviceWriteArgs<T>> BeforeDeviceWrite;
        public event EventHandler<DeviceWriteArgs<T>> AfterDeviceWrite;

        private void VerifyPointsAreInRange(ushort startingAddress, ushort numberOfPoints)
        {
            if (startingAddress + numberOfPoints > ushort.MaxValue)
                throw new ModbusServerException(ModbusExceptionCode.IllegalDataAddress);
        }

        T[] IDevicePointStorage<T>.ReadPoints(ushort startingAddress, ushort numberOfPoints)
        {
            VerifyPointsAreInRange(startingAddress, numberOfPoints);

            var args = new DeviceReadArgs(startingAddress, numberOfPoints);

            BeforeDeviceRead?.Invoke(this, args);

            var points = new T[numberOfPoints];

            for (var index = 0; index < numberOfPoints; index++)
            {
                values.TryGetValue((ushort)(startingAddress + index), out var value);

                points[index] = value;
            }

            AfterDeviceRead?.Invoke(this, args);

            return points;
        }

        void IDevicePointStorage<T>.WritePoints(ushort startingAddress, T[] values)
        {
            VerifyPointsAreInRange(startingAddress, (ushort)values.Length);

            var args = new DeviceWriteArgs<T>(startingAddress, values);

            BeforeDeviceWrite?.Invoke(this, args);

            for (var index = 0; index < values.Length; index++)
            {
                this.values[(ushort)(startingAddress + index)] = values[index];
            }

            AfterDeviceWrite?.Invoke(this, args);
        }
    }
}
