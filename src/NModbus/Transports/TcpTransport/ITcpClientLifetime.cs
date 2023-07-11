namespace NModbus.Transports.TcpTransport
{
    public interface ITcpClientLifetime : IAsyncDisposable
    {
        Task<ITcpClientContainer> GetTcpClientAsync(CancellationToken cancellationToken);
    }
}
