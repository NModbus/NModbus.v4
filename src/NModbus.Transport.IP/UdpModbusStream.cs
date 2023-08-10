using NModbus.Interfaces;
using System.Net.Sockets;

namespace NModbus.Transport.IP
{
    public class UdpModbusStream : IModbusStream
    {
        private readonly UdpClient udpClient;
        private readonly byte[] receiveBuffer = new byte[ushort.MaxValue];
        private int bufferOffset;

        public UdpModbusStream(UdpClient udpClient)
        {
            this.udpClient = udpClient ?? throw new ArgumentNullException(nameof(udpClient));
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

            if (bufferOffset == 0)
            {
                bufferOffset = udpClient.Client.Receive(receiveBuffer);
            }

            Array.Copy(receiveBuffer, 0, buffer, offset, count);
            bufferOffset -= count;
            Buffer.BlockCopy(receiveBuffer, count, receiveBuffer, 0, bufferOffset);

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

            await udpClient.SendAsync(datagram, count);
        }

        public ValueTask DisposeAsync()
        {
            udpClient.Dispose();
            return default;
        }
    }
}
