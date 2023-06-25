using Microsoft.Extensions.Logging;
using NModbus;
using NModbus.Transports.TcpTransport;
using System.Net.Sockets;

var loggerFactory = LoggerFactory.Create(builder =>
{
    builder
        .SetMinimumLevel(LogLevel.Debug)
        .AddConsole();
});

var logger = loggerFactory.CreateLogger<Program>();

//The unit number of the modbus server
const byte unitNumber = 1;

var tcpClient = new TcpClient();

await tcpClient.ConnectAsync("localhost", ModbusDefaultTcpPorts.Insecure);

await using var transport = new ModbusTcpClientTransport(tcpClient, loggerFactory);

var modbusClient = new ModbusClient(transport, loggerFactory);

logger.LogInformation("Writing a single register...");

await modbusClient.WriteSingleRegisterAsync(unitNumber, 0, 44);

{
    var holdingRegisters = await modbusClient.ReadHoldingRegistersAsync(unitNumber, 0, 5);

    logger.LogInformation("Read Holding Registers: {Registers}", string.Join(", ", holdingRegisters.Select(r => r.ToString())));
}

logger.LogInformation("Write multiple registers..");

await modbusClient.WriteMultipleRegistersAsync(unitNumber, 0, new ushort[] { 42, 43, 44 });

{
    var holdingRegisters = await modbusClient.ReadHoldingRegistersAsync(unitNumber, 0, 5);

    logger.LogInformation("Read Holding Registers: {Registers}", string.Join(", ", holdingRegisters.Select(r => r.ToString())));
}

Console.WriteLine("Press any key to exit...");
Console.ReadKey();


