namespace NModbus.Transport.Tcp
{
    public interface ITcpClientContainer : IAsyncDisposable
    {
        Stream Stream { get; }
    }
}
