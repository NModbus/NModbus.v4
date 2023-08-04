using NModbus.Interfaces;
using System.Net;
using System.Net.Sockets;

namespace NModbus.Transport.IP
{
    public class UdpStreamFactory : IStreamFactory
    {
        private readonly IPEndPoint endPoint;
        private readonly Action<UdpClient> configure;

        public UdpStreamFactory(IPEndPoint endPoint, Action<UdpClient> configure = null)
        {
            this.endPoint = endPoint ?? throw new ArgumentNullException(nameof(endPoint));
            this.configure = configure;
        }

        public Task<IModbusStream> CreateAndConnectAsync(CancellationToken cancellationToken)
        {
            var udpClient = new UdpClient();

            configure?.Invoke(udpClient);

            udpClient.Connect(endPoint);

            var stream = new UdpModbusStream(udpClient);


            return Task.FromResult<IModbusStream>(stream);
        }
    }
}
