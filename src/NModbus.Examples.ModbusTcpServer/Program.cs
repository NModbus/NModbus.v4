using Microsoft.Extensions.Logging;
using NModbus;
using NModbus.BasicServer;
using NModbus.Transports.TcpTransport;
using System.Net;
using System.Net.Sockets;

var loggerFactory = LoggerFactory.Create(builder =>
{
    builder
        .SetMinimumLevel(LogLevel.Debug)
        .AddConsole();
});

var serverNetwork = new ModbusServerNetwork(loggerFactory);

var tcpListener = new TcpListener(IPAddress.Loopback, ModbusDefaultTcpPorts.Insecure);

await using var transport = new ModbusTcpServerNetworkTransport(tcpListener, serverNetwork, loggerFactory);

var storage = new Storage();

var serverFunctions = ServerFunctionFactory.CreateBasicServerFunctions(storage, loggerFactory);

var server = new ModbusServer(1, serverFunctions, loggerFactory);

if (!serverNetwork.TryAddServer(server))
{
    Console.WriteLine("Unable to add server.");
}

Console.WriteLine("Awaiting requests... Press any key to exit.");
Console.ReadKey();