namespace NModbus.Transport.Tcp.ConnectionStrategies
{
    internal class PerRequestTcpClientRequestContainer : ITcpClientRequestContainer
    {
        private readonly TcpClientWrapper tcpClientWrapper;

        internal PerRequestTcpClientRequestContainer(TcpClientWrapper tcpClientWrapper)
        {
            this.tcpClientWrapper = tcpClientWrapper ?? throw new ArgumentNullException(nameof(tcpClientWrapper));
            Stream = tcpClientWrapper.Stream;
        }

        public Stream Stream { get; }

        public ValueTask DisposeAsync()
        {
            tcpClientWrapper.Dispose();
            return default;
        }
    }
}
