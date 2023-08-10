using NModbus.Interfaces;
using System.Net.Sockets;

namespace NModbus.Transport.IP
{
    public class TcpModbusStream : IModbusStream
    {
        private readonly TcpClient tcpClient;
        private readonly Stream stream;

        public TcpModbusStream(TcpClient tcpClient, Stream stream)
        {
            this.tcpClient = tcpClient ?? throw new ArgumentNullException(nameof(tcpClient));
            this.stream = stream ?? throw new ArgumentNullException(nameof(stream));
        }

        public async Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken = default)
        {
            return await stream.ReadAsync(buffer, offset, count, cancellationToken);
        }

        public async Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken = default)
        {
            await stream.WriteAsync(buffer, offset, count, cancellationToken);
        }

        public ValueTask DisposeAsync()
        {
            tcpClient.Dispose();
            stream.Dispose();

            return default;
        }
    }
}
