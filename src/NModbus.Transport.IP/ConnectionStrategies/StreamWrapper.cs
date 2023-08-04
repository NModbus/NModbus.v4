//using System.Net.Security;
//using System.Net.Sockets;

//namespace NModbus.Transport.IP.ConnectionStrategies
//{
//    /// <summary>
//    /// A wrapper for <see cref="TcpClient"/>. This is structured so that the TcpClient instance and the Stream can be disposed at the end of their lifetime.
//    /// </summary>
//    public class StreamWrapper : IDisposable
//    {
//        private readonly IDisposable[] disposables;

//        /// <summary>
//        /// Constructor.
//        /// </summary>
//        /// <param name="stream">Either the stream that was returned from <see cref="TcpClient.GetStream"/> or another wrapping stream (such as <see cref="SslStream"/>).</param>
//        /// <param name="disposables">Objects that should be disposed at the same time as the stream. In practice,
//        /// this is typically a <see cref="TcpClient"/> instance.</param>
//        /// <exception cref="ArgumentNullException"></exception>
//        public StreamWrapper(Stream stream, params IDisposable[] disposables)
//        {
//            Stream = stream ?? throw new ArgumentNullException(nameof(stream));
//            this.disposables = disposables;
//        }

//        /// <summary>
//        /// The stream associated with this TcpClient
//        /// </summary>
//        public Stream Stream { get; }

//        public void Dispose()
//        {
//            Stream.Dispose();

//            if (disposables != null)
//            {
//                foreach (var disposable in disposables)
//                {
//                    disposable.Dispose();
//                }
//            }

//            GC.SuppressFinalize(this);
//        }
//    }

//}
