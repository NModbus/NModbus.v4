namespace NModbus.BasicServer.Interfaces
{
    public class DeviceReadArgs : EventArgs
    {
        public DeviceReadArgs(ushort startingAddress, ushort numberOfPoints)
        {
            StartingAddress = startingAddress;
            NumberOfPoints = numberOfPoints;
        }

        public ushort StartingAddress { get; }

        public ushort NumberOfPoints { get; }
    }
}
