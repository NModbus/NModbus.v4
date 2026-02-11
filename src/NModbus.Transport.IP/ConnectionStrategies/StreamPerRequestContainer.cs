using NModbus.Interfaces;

namespace NModbus.Transport.IP.ConnectionStrategies;

internal class StreamPerRequestContainer : IPerRequestStreamContainer
{
    internal StreamPerRequestContainer(IModbusStream stream)
    {
        Stream = stream ?? throw new ArgumentNullException(nameof(stream));
    }

    public IModbusStream Stream { get; }

    public async ValueTask DisposeAsync()
    {
        await Stream.DisposeAsync();
    }
}
