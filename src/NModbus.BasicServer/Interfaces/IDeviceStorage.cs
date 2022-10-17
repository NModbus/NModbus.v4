namespace NModbus.BasicServer.Interfaces
{
    public interface IDeviceStorage
    {
        IDevicePointStorage<ushort> HoldingRegisters { get; }

        IDevicePointStorage<ushort> InputRegisters { get; }

        IDevicePointStorage<bool> Coils { get; }

        IDevicePointStorage<bool> DiscreteInputs { get; }
    }
}
