using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NModbus.BasicServer;
using NModbus.Extensions;
using NModbus.Tests.Transport;
using Shouldly;

namespace NModbus.Tests.Integration
{
    public class WriteSingleRegisterShouldWork
    {
        private readonly ILoggerFactory loggerFactory = new NullLoggerFactory();

        [Fact]
        public async Task WriteSingleRegister()
        {
            var storage = new Storage();

            var serverFunctions = ServerFunctionFactory.CreateBasicServerFunctions(loggerFactory, storage);

            var server = new ModbusServer(1, serverFunctions, loggerFactory.CreateLogger<ModbusServer>());

            using var streamA = new MemoryStream();
            using var streamB = new MemoryStream();

            var serverTransport = new TestingTransport(streamA, streamB);
            var clientTransport = new TestingTransport(streamB, streamA);

            var serverNetwork = new ModbusServerNetwork(serverTransport);

            serverNetwork.AddServer(server);

            var client = new ModbusClient(clientTransport, loggerFactory.CreateLogger<ModbusClient>());

            await client.WriteSingleRegisterAsync(1, 10, 42);

            storage.HoldingRegisters[10].ShouldBe((ushort)42);
        }
    }
}
