using Microsoft.Extensions.Logging.Abstractions;
using NModbus;
using NModbus.Extensions;
using NModbus.Functions;
using NModbus.Transports.TcpTransport;
using System.Net.Sockets;

using var tcpClient = new TcpClient();

await tcpClient.ConnectAsync("localhost", 502);

var transport = new ModbusTcpTransport(tcpClient, new NullLogger<ModbusTcpTransport>());

var clientFunctions = new IClientFunction[]
{
    new ModbusClientFunction<WriteSingleRegisterRequest, WriteSingleRegisterResponse>(ModbusFunctionCodes.WriteSingleRegister, new WriteSingleRegisterMessageFactory())
};

var modbusClient = new ModbusClient(clientFunctions, transport, new NullLogger<ModbusClient>());

await modbusClient.WriteSingleRegisterAsync(1, 0, 42);