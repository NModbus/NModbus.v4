using NModbus.Interfaces;
using System.IO.Ports;

namespace NModbus.Transport.Serial
{
    public class SerialModbusStream : IModbusStream
    {
        private const string NewLine = "\r\n";
        private readonly SerialPort serialPort;

        public SerialModbusStream(SerialPort serialPort)
        {
            this.serialPort = serialPort;
            this.serialPort.NewLine = NewLine;
        }

        public Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken = default)
        {
            var read = serialPort.Read(buffer, offset, count);

            return Task.FromResult(read);
        }

        public Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken = default)
        {
            serialPort.Write(buffer, offset, count);

            return Task.CompletedTask;
        }

        public ValueTask DisposeAsync()
        {
            serialPort.Dispose();
            GC.SuppressFinalize(this);
            return default;
        }
    }
}
