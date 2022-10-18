namespace NModbus.BasicServer.Interfaces
{
    public class DeviceWriteArgs<T> : EventArgs
    {
        public DeviceWriteArgs(ushort startingAddress, T[] points)
        {
            StartingAddress = startingAddress;
            Points = points;
        }

        public ushort StartingAddress { get; }

        public T[] Points { get; }
    }
}
