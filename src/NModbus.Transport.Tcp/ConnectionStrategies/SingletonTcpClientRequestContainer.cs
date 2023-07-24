namespace NModbus.Transport.Tcp.ConnectionStrategies
{
    internal class SingletonTcpClientRequestContainer : ITcpClientRequestContainer
    {
        public SingletonTcpClientRequestContainer(TcpClientWrapper tcpClientWrapper)
        {
            if (tcpClientWrapper is null) throw new ArgumentNullException(nameof(tcpClientWrapper));

            Stream = tcpClientWrapper.Stream;
        }

        public Stream Stream { get; }

        public ValueTask DisposeAsync()
        {
            return default;
        }
    }
}
