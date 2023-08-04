using NModbus.Interfaces;

namespace NModbus.Transport.IP.ConnectionStrategies
{
    internal class StreamPerRequestContainer : IPerRequestStreamContainer
    {
        private readonly IModbusStream stream;

        internal StreamPerRequestContainer(IModbusStream stream)
        {
            this.stream = stream ?? throw new ArgumentNullException(nameof(stream));
        }

        public IModbusStream Stream { get; }

        public async ValueTask DisposeAsync()
        {
            await stream.DisposeAsync();
        }
    }
}
