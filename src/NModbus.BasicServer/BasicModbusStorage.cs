using NModbus.BasicServer.Interfaces;

namespace NModbus.BasicServer
{
    public class BasicModbusStorage : IBasicModbusStorage
    {
        public IBasicPointStorage<ushort> HoldingRegisters { get; }

        public IBasicPointStorage<ushort> InputRegisters { get; }

        public IBasicPointStorage<bool> Coils { get; }

        public IBasicPointStorage<bool> DiscreteInputs { get; }
    }
}
