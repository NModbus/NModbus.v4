using NModbus.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace NModbus
{
    /// <summary>
    /// Simple implementation of <see cref="IModbusStream"/> that wraps a generic <see cref="Stream"/>.
    /// </summary>
    public class ModbusStream : IModbusStream
    {
        private readonly Stream stream;

        public int ReceiveTimeout 
        {
            get => stream.ReadTimeout;
            set => stream.ReadTimeout = value;
        }
        public int SendTimeout 
        {
            get => stream.WriteTimeout;
            set => stream.WriteTimeout = value;
        }

        public ModbusStream(Stream stream)
        {
            this.stream = stream ?? throw new ArgumentNullException(nameof(stream));
        }

        public async Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken = default)
        {
            return await stream.ReadAsync(buffer, offset, count, cancellationToken).ConfigureAwait(false);
        }

        public async Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken = default)
        {
            await stream.WriteAsync(buffer, offset, count, cancellationToken);
        }

        public async ValueTask DisposeAsync()
        {
            await stream.DisposeAsync().ConfigureAwait(false);
        }
    }
}
