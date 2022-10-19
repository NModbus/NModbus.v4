using NModbus.BasicServer.Interfaces;

namespace NModbus.BasicServer
{
    public class Storage : IDeviceStorage
    {
        private readonly IPointStorage<ushort> holdingRegisters;
        private readonly IPointStorage<ushort> inputRegisters;
        private readonly IPointStorage<bool> coils;
        private readonly IPointStorage<bool> discreteInputs;

        public Storage(
            IPointStorage<ushort> holdingRegisters = null,
            IPointStorage<ushort> inputRegisters = null,
            IPointStorage<bool> coils = null,
            IPointStorage<bool> discreteInputs = null)
        {
            this.holdingRegisters = holdingRegisters ?? new SparsePointStorage<ushort>();
            this.inputRegisters = inputRegisters ?? new SparsePointStorage<ushort>();
            this.coils = coils ?? new SparsePointStorage<bool>();
            this.discreteInputs = discreteInputs ?? new SparsePointStorage<bool>();
        }

        public IApplicationPointStorage<ushort> HoldingRegisters => holdingRegisters;

        public IApplicationPointStorage<ushort> InputRegisters => inputRegisters;

        public IApplicationPointStorage<bool> Coils => coils;

        public IApplicationPointStorage<bool> DiscreteInputs => discreteInputs;

        IDevicePointStorage<ushort> IDeviceStorage.HoldingRegisters => holdingRegisters;

        IDevicePointStorage<ushort> IDeviceStorage.InputRegisters => inputRegisters;

        IDevicePointStorage<bool> IDeviceStorage.Coils => coils;

        IDevicePointStorage<bool> IDeviceStorage.DiscreteInputs => discreteInputs;
    }
}
