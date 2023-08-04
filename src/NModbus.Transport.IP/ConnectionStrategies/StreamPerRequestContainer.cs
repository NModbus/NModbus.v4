namespace NModbus.Transport.IP.ConnectionStrategies
{
    internal class StreamPerRequestContainer : IPerRequestStreamContainer
    {
        private readonly StreamWrapper tcpClientWrapper;

        internal StreamPerRequestContainer(StreamWrapper tcpClientWrapper)
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
