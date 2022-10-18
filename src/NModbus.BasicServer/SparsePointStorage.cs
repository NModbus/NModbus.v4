using NModbus.BasicServer.Interfaces;
using NModbus.Interfaces;

namespace NModbus.BasicServer
{
    public class SparsePointStorage<T> : IDevicePointStorage<T>, IApplicationPointStorage<T>
    {
        private readonly Dictionary<ushort, T> values = new Dictionary<ushort, T>();

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
                values.TryGetValue((ushort)(startingAddress + numberOfPoints), out var value);

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
