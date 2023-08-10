using NModbus.Interfaces;

namespace NModbus.Transport.IP.ConnectionStrategies
{
    /// <summary>
    /// Does not dispose the <see cref="IModbusStream"/> instance.
    /// </summary>
    internal class SingletonStreamPerRequestContainer : IPerRequestStreamContainer
    {
        public SingletonStreamPerRequestContainer(IModbusStream stream)
        {
            Stream = stream ?? throw new ArgumentNullException(nameof(stream));
        }

        public IModbusStream Stream { get; }

        public ValueTask DisposeAsync()
        {
            //Do not dispose of the streamWrapper instance. It's a singleton and will be used for other requests.
            return default;
        }
    }
}
