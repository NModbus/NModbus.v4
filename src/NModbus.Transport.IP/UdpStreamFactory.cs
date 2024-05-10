using NModbus.Interfaces;
using System.Net;
using System.Net.Sockets;

namespace NModbus.Transport.IP
{
    public class UdpStreamFactory : IStreamFactory
    {
        private readonly IPEndPoint _endPoint;
        private readonly Action<UdpClient> _configure;

        public UdpStreamFactory(IPEndPoint endPoint, Action<UdpClient> configure = null)
        {
            _endPoint = endPoint ?? throw new ArgumentNullException(nameof(endPoint));
            _configure = configure;
        }

        public Task<IModbusStream> CreateAndConnectAsync(CancellationToken cancellationToken)
        {
            var udpClient = new UdpClient();

            _configure?.Invoke(udpClient);

            udpClient.Connect(_endPoint);

            var stream = new UdpModbusStream(udpClient);


            return Task.FromResult<IModbusStream>(stream);
        }
    }
}
