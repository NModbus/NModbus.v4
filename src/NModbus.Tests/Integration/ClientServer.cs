using Microsoft.Extensions.Logging;
using NModbus.BasicServer;
using NModbus.Interfaces;
using NModbus.Transports.TcpTransport;
using System.Net;
using System.Net.Sockets;

namespace NModbus.Tests.Integration
{
    public class ClientServer : IAsyncDisposable
    {
        private readonly ILoggerFactory loggerFactory;
        private readonly ModbusTcpServerNetworkTransport serverTransport;
        private readonly IModbusClientTransport clientTransport;
        private readonly IModbusServerNetwork serverNetwork;

        public ClientServer(byte unitNumber, ILoggerFactory loggerFactory)
        {
            UnitNumber = unitNumber;
            this.loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));

            //Create the server
            serverNetwork = new ModbusServerNetwork(loggerFactory);

            var serverFunctions = ServerFunctionFactory.CreateBasicServerFunctions(Storage, loggerFactory);

            var server = new ModbusServer(UnitNumber, serverFunctions, loggerFactory);

            if (!serverNetwork.TryAddServer(server))
                throw new InvalidOperationException($"Unable to add server with unit number {server.UnitNumber}");

            var tcpListener = new TcpListener(IPAddress.Loopback, ModbusDefaultTcpPorts.Insecure);

            serverTransport = new ModbusTcpServerNetworkTransport(tcpListener, serverNetwork, loggerFactory);

            //Create the client
            var tcpClient = new TcpClient("127.0.0.1", ModbusDefaultTcpPorts.Insecure);
            var strategy = new SimpleTcpClientLifetime(tcpClient.GetStream());
            clientTransport = new ModbusTcpClientTransport(strategy, loggerFactory);
            Client = new ModbusClient(clientTransport, loggerFactory);
        }

        public byte UnitNumber { get; }

        public IModbusClient Client { get; }

        public Storage Storage { get; } = new Storage();

        public async ValueTask DisposeAsync()
        {
            await serverTransport.DisposeAsync();
            await clientTransport.DisposeAsync();
        }
    }
}
