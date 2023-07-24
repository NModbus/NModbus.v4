using NModbus.Transport.Tcp.ConnectionStrategies;

namespace NModbus.Transport.Tcp
{
    public interface ITcpClientFactory
    {
        Task<TcpClientWrapper> CreateAndConnectAsync(CancellationToken cancellationToken);
    }
}