namespace NModbus.Transport.Tcp
{
    public interface ITcpClientLifetime : IAsyncDisposable
    {
        Task<ITcpClientContainer> GetTcpClientAsync(CancellationToken cancellationToken);
    }
}
