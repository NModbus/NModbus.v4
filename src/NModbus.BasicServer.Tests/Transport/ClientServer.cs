using Microsoft.Extensions.Logging;
using NModbus.Interfaces;
using NModbus.Transport.Tcp;
using NModbus.Transport.Tcp.ConnectionStrategies;
using System.Net;
using System.Net.Sockets;

namespace NModbus.BasicServer.Tests.Transport
{
    public class ClientServer : IAsyncDisposable
    {
        private readonly ModbusTcpServerNetworkTransport serverTransport;
        private readonly IModbusClientTransport clientTransport;
        private readonly IModbusServerNetwork serverNetwork;

        public ClientServer(byte unitIdentifier, ILoggerFactory loggerFactory)
        {
            if (loggerFactory is null) throw new ArgumentNullException(nameof(loggerFactory));

            UnitIdentifier = unitIdentifier;

            //Create the server
            serverNetwork = new ModbusServerNetwork(loggerFactory);

            var serverFunctions = ServerFunctionFactory.CreateBasicServerFunctions(Storage, loggerFactory);

            var server = new ModbusServer(UnitIdentifier, serverFunctions, loggerFactory);

            if (!serverNetwork.TryAddServer(server))
                throw new InvalidOperationException($"Unable to add server with unit number {server.UnitIdentifier}");

            var tcpListener = new TcpListener(IPAddress.Loopback, ModbusTcpPorts.Insecure);

            serverTransport = new ModbusTcpServerNetworkTransport(tcpListener, serverNetwork, loggerFactory);

            var tcpClientFactory = new TcpClientFactory(IPAddress.Loopback);

            //Create the client
            var tcpClientLifetime = new SingletonTcpClientConnectionStrategy(tcpClientFactory, loggerFactory);
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

            GC.SuppressFinalize(this);
        }
    }
}
