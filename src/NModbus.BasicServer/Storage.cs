using NModbus.BasicServer.Interfaces;

namespace NModbus.BasicServer
{
    public class Storage : IDeviceStorage
    {
        private readonly IPointStorage<ushort> _holdingRegisters;
        private readonly IPointStorage<ushort> _inputRegisters;
        private readonly IPointStorage<bool> _coils;
        private readonly IPointStorage<bool> _discreteInputs;

        public Storage(
            IPointStorage<ushort> holdingRegisters = null,
            IPointStorage<ushort> inputRegisters = null,
            IPointStorage<bool> coils = null,
            IPointStorage<bool> discreteInputs = null)
        {
            _holdingRegisters = holdingRegisters ?? new SparsePointStorage<ushort>();
            _inputRegisters = inputRegisters ?? new SparsePointStorage<ushort>();
            _coils = coils ?? new SparsePointStorage<bool>();
            _discreteInputs = discreteInputs ?? new SparsePointStorage<bool>();
        }

        public IApplicationPointStorage<ushort> HoldingRegisters => _holdingRegisters;

        public IApplicationPointStorage<ushort> InputRegisters => _inputRegisters;

        public IApplicationPointStorage<bool> Coils => _coils;

        public IApplicationPointStorage<bool> DiscreteInputs => _discreteInputs;

        IDevicePointStorage<ushort> IDeviceStorage.HoldingRegisters => _holdingRegisters;

        IDevicePointStorage<ushort> IDeviceStorage.InputRegisters => _inputRegisters;

        IDevicePointStorage<bool> IDeviceStorage.Coils => _coils;

        IDevicePointStorage<bool> IDeviceStorage.DiscreteInputs => _discreteInputs;
    }
}
