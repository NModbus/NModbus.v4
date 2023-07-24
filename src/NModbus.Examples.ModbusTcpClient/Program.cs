﻿using Microsoft.Extensions.Logging;
using NModbus;
using NModbus.Transport.Tcp;
using NModbus.Transport.Tcp.ConnectionStrategies;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

var loggerFactory = LoggerFactory.Create(builder =>
{
    builder
        .SetMinimumLevel(LogLevel.Debug)
        .AddConsole();
});

var logger = loggerFactory.CreateLogger<Program>();

//The unit number of the modbus server
const byte unitIdentifier = 1;

// options = null

#if FALSE

var tcpClientFactory = new TcpClientFactory(IPAddress.Loopback, ModbusTcpPorts.Insecure);

var strategy = new SingletonTcpClientConnectionStrategy(tcpClientFactory, loggerFactory);

#else

var options = new SslClientAuthenticationOptions
{
    TargetHost = "127.0.0.1",
    EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12 | System.Security.Authentication.SslProtocols.Tls13,
    RemoteCertificateValidationCallback = RemoteCertificateValidationCallback
};

var tcpClientFactory = new TcpClientFactory(IPAddress.Loopback, ModbusTcpPorts.Secure, null, options);

var strategy = new SingletonTcpClientConnectionStrategy(tcpClientFactory, loggerFactory);

#endif

await using var transport = new ModbusTcpClientTransport(strategy, loggerFactory);

var modbusClient = new ModbusClient(transport, loggerFactory);

logger.LogInformation("Writing a single register...");

await modbusClient.WriteSingleRegisterAsync(unitIdentifier, 0, 44);

{
    var holdingRegisters = await modbusClient.ReadHoldingRegistersAsync(unitIdentifier, 0, 5);

    logger.LogInformation("Read Holding Registers: {Registers}", string.Join(", ", holdingRegisters.Select(r => r.ToString())));
}

logger.LogInformation("Write multiple registers..");

await modbusClient.WriteMultipleRegistersAsync(unitIdentifier, 0, new ushort[] { 42, 43, 44 });

{
    var holdingRegisters = await modbusClient.ReadHoldingRegistersAsync(unitIdentifier, 0, 5);

    logger.LogInformation("Read Holding Registers: {Registers}", string.Join(", ", holdingRegisters.Select(r => r.ToString())));
}

Console.WriteLine("Press any key to exit...");
Console.ReadKey();


static bool RemoteCertificateValidationCallback(object sender, X509Certificate? certificate, X509Chain? chain, SslPolicyErrors sslPolicyErrors)
{
    return true;
}