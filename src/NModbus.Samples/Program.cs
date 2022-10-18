using Microsoft.Extensions.Logging.Abstractions;
using NModbus;
using NModbus.Extensions;
using NModbus.Transports.TcpTransport;
using System.Net.Sockets;

using var tcpClient = new TcpClient();

await tcpClient.ConnectAsync("localhost", 502);

var transport = new ModbusTcpTransport(tcpClient, new NullLogger<ModbusTcpTransport>());

var modbusClient = new ModbusClient(transport, new NullLogger<ModbusClient>());

//await modbusClient.WriteSingleRegisterAsync(1, 0, 44);

//var holdingRegisters = await modbusClient.ReadHoldingRegistersAsync(1, 0, 5);

//var index = 0;

//foreach(var holdingRegister in holdingRegisters)
//{
//    Console.WriteLine($"[{index}]: {holdingRegister}");

//    index++;
//}

await modbusClient.WriteMultipleRegistersAsync(1, 0, new ushort[] { 42, 43, 44 });