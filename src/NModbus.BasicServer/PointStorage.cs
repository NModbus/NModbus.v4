using NModbus.BasicServer.Interfaces;

namespace NModbus.BasicServer
{
    public class PointStorage : IDeviceStorage
    {
        public IDevicePointStorage<ushort> HoldingRegisters { get; }

        public IDevicePointStorage<ushort> InputRegisters { get; }

        public IDevicePointStorage<bool> Coils { get; }

        public IDevicePointStorage<bool> DiscreteInputs { get; }
    }
}
