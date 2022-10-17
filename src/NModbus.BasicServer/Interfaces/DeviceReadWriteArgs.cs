namespace NModbus.BasicServer.Interfaces
{
    public class DeviceReadWriteArgs : EventArgs
    {
        public DeviceReadWriteArgs(ushort startingAddress, ushort numberOfPoints)
        {
            StartingAddress = startingAddress;
            NumberOfPoints = numberOfPoints;
        }

        public ushort StartingAddress { get; }

        public ushort NumberOfPoints { get; }
    }
}
