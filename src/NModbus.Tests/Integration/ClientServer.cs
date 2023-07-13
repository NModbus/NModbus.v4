using Microsoft.Extensions.Logging;
using NModbus.BasicServer;
using NModbus.Interfaces;
using NModbus.Transport.Tcp;
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

        public ClientServer(byte unitIdentifier, ILoggerFactory loggerFactory)
        {
            UnitIdentifier = unitIdentifier;
            this.loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));

            //Create the server
            serverNetwork = new ModbusServerNetwork(loggerFactory);

            var serverFunctions = ServerFunctionFactory.CreateBasicServerFunctions(Storage, loggerFactory);

            var server = new ModbusServer(UnitIdentifier, serverFunctions, loggerFactory);

            if (!serverNetwork.TryAddServer(server))
                throw new InvalidOperationException($"Unable to add server with unit number {server.UnitIdentifier}");

            var tcpListener = new TcpListener(IPAddress.Loopback, ModbusTcpPorts.Insecure);

            serverTransport = new ModbusTcpServerNetworkTransport(tcpListener, serverNetwork, loggerFactory);

            //Create the client
            var tcpClient = new TcpClient("127.0.0.1", ModbusTcpPorts.Insecure);
            var tcpClientLifetime = new SimpleTcpClientLifetime(tcpClient.GetStream());
            clientTransport = new ModbusTcpClientTransport(tcpClientLifetime, loggerFactory);
            Client = new ModbusClient(clientTransport, loggerFactory);
        }

        public byte UnitIdentifier { get; }

        public IModbusClient Client { get; }

        public Storage Storage { get; } = new Storage();

        public async ValueTask DisposeAsync()
        {
            await serverTransport.DisposeAsync();
            await clientTransport.DisposeAsync();
        }
    }
}
