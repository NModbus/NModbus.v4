using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NModbus;
using NModbus.Transports.TcpTransport;
using System.Net.Sockets;

var loggerFactory = LoggerFactory.Create(builder =>
{
    builder
        .SetMinimumLevel(LogLevel.Debug)
        .AddConsole();
});

//The unit number of the modbus server
const byte unitNumber = 1;

var tcpClient = new TcpClient();

await tcpClient.ConnectAsync("localhost", ModbusDefaultTcpPorts.Insecure);

await using var transport = new ModbusTcpClientTransport(tcpClient, loggerFactory);

var modbusClient = new ModbusClient(transport, loggerFactory);

Console.WriteLine("Writing a single register...");

await modbusClient.WriteSingleRegisterAsync(unitNumber, 0, 44);

{
    var index = 0;

    Console.WriteLine("Read Holding Registers...");

    var holdingRegisters = await modbusClient.ReadHoldingRegistersAsync(unitNumber, 0, 5);

    foreach (var holdingRegister in holdingRegisters)
    {
        Console.WriteLine($"[{index}]: {holdingRegister}");

        index++;
    }
}

Console.WriteLine("Write multiple registers..");

await modbusClient.WriteMultipleRegistersAsync(unitNumber, 0, new ushort[] { 42, 43, 44 });

{
    var index = 0;

    Console.WriteLine("Read Holding Registers...");

    var holdingRegisters = await modbusClient.ReadHoldingRegistersAsync(unitNumber, 0, 5);

    foreach (var holdingRegister in holdingRegisters)
    {
        Console.WriteLine($"[{index}]: {holdingRegister}");

        index++;
    }
}

Console.WriteLine("Press any key to exit...");
Console.ReadKey();