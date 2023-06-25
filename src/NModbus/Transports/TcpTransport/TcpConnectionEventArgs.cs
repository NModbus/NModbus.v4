namespace NModbus.Transports.TcpTransport
{
    internal class TcpConnectionEventArgs : EventArgs
    {
        public TcpConnectionEventArgs(string endpoint)
        {
            if (string.IsNullOrEmpty(endpoint))
            {
                throw new ArgumentException($"'{nameof(endpoint)}' cannot be null or empty.", nameof(endpoint));
            }

            Endpoint = endpoint;
        }

        public string Endpoint { get; }
    }
}
