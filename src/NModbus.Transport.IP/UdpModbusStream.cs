using NModbus.Interfaces;
using System.Net.Sockets;

namespace NModbus.Transport.IP
{
    public class UdpModbusStream : IModbusStream
    {
        private readonly UdpClient _udpClient;
        private readonly byte[] _receiveBuffer = new byte[ushort.MaxValue];
        private int _bufferOffset;

        public UdpModbusStream(UdpClient udpClient)
        {
            _udpClient = udpClient ?? throw new ArgumentNullException(nameof(udpClient));
        }

        public Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken = default)
        {
            if (buffer == null) throw new ArgumentNullException(nameof(buffer));

            if (offset < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(offset),
                    "Argument offset must be greater than or equal to 0.");
            }

            if (offset > buffer.Length)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(offset),
                    "Argument offset cannot be greater than the length of buffer.");
            }

            if (count < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(count),
                    "Argument count must be greater than or equal to 0.");
            }

            if (count > buffer.Length - offset)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(count),
                    "Argument count cannot be greater than the length of buffer minus offset.");
            }

            if (_bufferOffset == 0)
            {
                _bufferOffset = _udpClient.Client.Receive(_receiveBuffer);
            }

            Array.Copy(_receiveBuffer, 0, buffer, offset, count);
            _bufferOffset -= count;
            Buffer.BlockCopy(_receiveBuffer, count, _receiveBuffer, 0, _bufferOffset);

            return Task.FromResult(count);
        }

        public async Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken = default)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException(nameof(buffer));
            }

            if (offset < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(offset),
                    "Argument offset must be greater than or equal to 0.");
            }

            if (offset > buffer.Length)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(offset),
                    "Argument offset cannot be greater than the length of buffer.");
            }

            if (count < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(count),
                    "Argument count must be greater than or equal to 0.");
            }

            if (count > buffer.Length - offset)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(count),
                    "Argument count cannot be greater than the length of buffer minus offset.");
            }

            var datagram = new byte[count];

            Array.Copy(buffer, offset, datagram, 0, count);

            await _udpClient.SendAsync(datagram, count);
        }

        public ValueTask DisposeAsync()
        {
            _udpClient.Dispose();
            return default;
        }
    }
}
