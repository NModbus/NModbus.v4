using NModbus.BasicServer.Interfaces;

namespace NModbus.BasicServer
{
    public class BasicPointStorage<T> : IBasicPointStorage<T>
    {
        private readonly Dictionary<ushort, T> values = new Dictionary<ushort, T>();

        public Task<T> ReadPointAsync(ushort address)
        {
            if (!values.TryGetValue(address, out T value))
            {
                value = default;
            }

            return Task.FromResult(value);
        }

        public Task WritePointAsync(ushort address, T value)
        {
            values[address] = value;

            return Task.CompletedTask;
        }
    }
}
