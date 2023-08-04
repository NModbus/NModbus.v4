using NModbus.Interfaces;
using System.Net;
using System.Net.Sockets;

namespace NModbus.Transport.IP
{
    public class ModbusUdpClientTransport : ModbusIPClientTransportBase
    {
        private readonly UdpClient udpClient;
        private readonly IPEndPoint endPoint;

        public ModbusUdpClientTransport(IPEndPoint endPoint)
        {
            udpClient = new UdpClient();
            this.endPoint = endPoint;
        }

        public override Task<IModbusDataUnit> SendAndReceiveAsync(IModbusDataUnit message, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async override Task SendAsync(IModbusDataUnit message, CancellationToken cancellationToken = default)
        {
            var transactionIdentifier = GetNextTransactionIdenfier();

            var bytes = ModbusIPMessageSerializer.Serialize(message, transactionIdentifier);

            await udpClient.SendAsync(bytes, bytes.Length, endPoint);
        }

        public override ValueTask DisposeAsync()
        {
            udpClient.Dispose();

            return default;
        }
    }
}
