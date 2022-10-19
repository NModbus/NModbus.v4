using NModbus.Extensions;
using NModbus.Interfaces;

namespace NModbus.Tests.Transport
{
    public class TestingTransport : IModbusTransport
    {
        private readonly Stream receiveStream;
        private readonly Stream transmitStream;

        public TestingTransport(Stream receiveStream, Stream transmitStream)
        {
            this.receiveStream = receiveStream ?? throw new ArgumentNullException(nameof(receiveStream));
            this.transmitStream = transmitStream ?? throw new ArgumentNullException(nameof(transmitStream));
        }

        public async Task SendAsync(ApplicationDataUnit applicationDataUnit, CancellationToken cancellationToken = default)
        {
            //Encode the number of bytes to be sent
            var numberOfBytes = (byte)(applicationDataUnit.ProtocolDataUnit.Length + 2);

            await transmitStream.WriteAsync(new byte[] { numberOfBytes, applicationDataUnit.UnitNumber }, cancellationToken);

            await transmitStream.WriteAsync(applicationDataUnit.ProtocolDataUnit.ToArray(), cancellationToken);
        }

        public async Task<ApplicationDataUnit> SendAndReceiveAsync(ApplicationDataUnit applicationDataUnit, CancellationToken cancellationToken = default)
        {
            await SendAsync(applicationDataUnit, cancellationToken);

            return await ReceiveAsync();
        }

        public async Task<ApplicationDataUnit> ReceiveAsync(CancellationToken cancellationToken = default)
        {
            var headerBuffer = new byte[1];

            var read = await receiveStream.ReadAsync(headerBuffer, 0, 1, cancellationToken);

            if (read != 1)
                throw new IOException("0 bytes were read.");

            var aduBuffer = new byte[headerBuffer[0]];

            await receiveStream.ReadBufferAsync(aduBuffer, cancellationToken);

            return new ApplicationDataUnit(aduBuffer);
        }
    }
}
