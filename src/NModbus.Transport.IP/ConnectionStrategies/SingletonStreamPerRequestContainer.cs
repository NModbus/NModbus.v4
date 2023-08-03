using NModbus.Transport.Tcp;

namespace NModbus.Transport.IP.ConnectionStrategies
{
    /// <summary>
    /// Does not dispose the <see cref="StreamWrapper"/> instance.
    /// </summary>
    internal class SingletonStreamPerRequestContainer : IPerRequestStreamContainer
    {
        public SingletonStreamPerRequestContainer(StreamWrapper streamWrapper)
        {
            if (streamWrapper is null) throw new ArgumentNullException(nameof(streamWrapper));

            Stream = streamWrapper.Stream;
        }

        public Stream Stream { get; }

        public ValueTask DisposeAsync()
        {
            //Do not dispose of the streamWrapper instance. It's a singleton and will be used for other requests.
            return default;
        }
    }
}
