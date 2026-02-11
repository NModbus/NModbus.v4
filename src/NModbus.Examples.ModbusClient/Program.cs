using Microsoft.Extensions.Logging;
using NModbus;
using NModbus.Examples.ModbusClient;
using NModbus.Transport.Serial;
using System.IO.Ports;
using System.Net;

ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
{
    builder
        .SetMinimumLevel(LogLevel.Debug)
        .AddConsole();
});

var logger = loggerFactory.CreateLogger<Program>();

var serialPort = new SerialPort("COM4");

await using var transport = new ModbusAsciiClientTransport(serialPort);

var modbusClient = new ModbusClient(transport, loggerFactory);

var registers = await modbusClient.ReadHoldingRegistersAsync(1, 1000, 10);

if (registers == null)
    throw new InvalidOperationException("No response");

foreach(var register in registers)
{
    Console.WriteLine($"{register}");
}

    
////The unit number of the modbus server
//const byte unitIdentifier = 1;

//var sampleFactory = new ModbusIpClientSampleTransportFactory(loggerFactory);

//string sample = "insecure";

//await using var transport = sample switch
//{
//    // create a "standard" modbus tcp client
//    "insecure" => sampleFactory.CreateTcpInsecureClient(IPAddress.Loopback),

//    // create a modbus secure client, accepting all certificates
//    "secure" => await sampleFactory.CreateTcpSecureClient("localhost", (snd, cert, chain, errors) => true),

//    // create a "standard" modbus upd client
//    "udp" => sampleFactory.CreateUpdClient(IPAddress.Loopback),

//    _ => throw new NotSupportedException("Only 'insecure', 'secure' or 'udp' is supported as option")
//};

//var modbusClient = new ModbusClient(transport, loggerFactory);

//logger.LogInformation("Writing a single register...");

//await modbusClient.WriteSingleRegisterAsync(unitIdentifier, 0, 44);
//{
//    var holdingRegisters = await modbusClient.ReadHoldingRegistersAsync(unitIdentifier, 0, 5);

//    if (holdingRegisters == null)
//    {
//        logger.LogInformation("No response.");
//    }
//    else
//    {
//        logger.LogInformation("Read Holding Registers: {Registers}", string.Join(", ", holdingRegisters.Select(r => r.ToString())));
//    }
//}

//logger.LogInformation("Write multiple registers..");

//await modbusClient.WriteMultipleRegistersAsync(unitIdentifier, 0, new ushort[] { 42, 43, 44 });

//{
//    var holdingRegisters = await modbusClient.ReadHoldingRegistersAsync(unitIdentifier, 0, 5);

//    if (holdingRegisters == null)
//    {
//        logger.LogInformation("No response."); 
//    }
//    else
//    {
//        logger.LogInformation("Read Holding Registers: {Registers}", string.Join(", ", holdingRegisters.Select(r => r.ToString())));
//    }
        
//}

//Console.WriteLine("Press any key to exit...");
//Console.ReadKey();


