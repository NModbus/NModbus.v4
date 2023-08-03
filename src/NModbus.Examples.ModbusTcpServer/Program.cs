using Microsoft.Extensions.Logging;
using NModbus;
using NModbus.BasicServer;
using NModbus.Transport.Tcp;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;

var loggerFactory = LoggerFactory.Create(builder =>
{
    builder
        .SetMinimumLevel(LogLevel.Debug)
        .AddConsole();
});

var serverNetwork = new ModbusServerNetwork(loggerFactory);

#if FALSE

var tcpListener = new TcpListener(IPAddress.Loopback, ModbusTcpPorts.Insecure);

await using var transport = new ModbusTcpServerNetworkTransport(tcpListener, serverNetwork, loggerFactory, options);

#else

var tcpListener = new TcpListener(IPAddress.Loopback, ModbusTcpPorts.Secure);

var options = new SslServerAuthenticationOptions
{
    ServerCertificate = X509Certificate.CreateFromCertFile("../../../../../certificates/modbus-test.pfx"),
    EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12 | System.Security.Authentication.SslProtocols.Tls12,
};

await using var transport = new ModbusTcpServerNetworkTransport(tcpListener, serverNetwork, loggerFactory, options);

#endif

var storage = new Storage();

var serverFunctions = ServerFunctionFactory.CreateBasicServerFunctions(storage, loggerFactory);

var server = new ModbusServer(1, serverFunctions, loggerFactory);

if (!serverNetwork.TryAddServer(server))
{
    Console.WriteLine("Unable to add server.");
}

Console.WriteLine("Awaiting requests... Press any key to exit.");
Console.ReadKey();