namespace NModbus.BasicServer.Interfaces
{
    public interface IBasicModbusStorage
    {
        IBasicPointStorage<ushort> HoldingRegisters { get; }

        IBasicPointStorage<ushort> InputRegisters { get; }

        IBasicPointStorage<bool> Coils { get; }

        IBasicPointStorage<bool> DiscreteInputs { get; }
    }
}
