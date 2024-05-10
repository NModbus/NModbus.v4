using NModbus.Interfaces;

namespace NModbus.Transport.IP.ConnectionStrategies
{
    internal class StreamPerRequestContainer : IPerRequestStreamContainer
    {
        private readonly IModbusStream _stream;

        internal StreamPerRequestContainer(IModbusStream stream)
        {
            _stream = stream ?? throw new ArgumentNullException(nameof(stream));
        }

        public IModbusStream Stream { get; }

        public async ValueTask DisposeAsync()
        {
            await _stream.DisposeAsync();
        }
    }
}
