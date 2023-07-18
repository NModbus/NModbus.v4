using Divergic.Logging.Xunit;
using Microsoft.Extensions.Logging;
using NModbus.BasicServer;
using NModbus.Extensions;
using Shouldly;
using Xunit.Abstractions;

namespace NModbus.Tests.Integration
{
    public abstract class ClientServerTestBase
    {
        protected readonly ILoggerFactory loggerFactory;

        protected ClientServerTestBase(ITestOutputHelper output)
        {
            loggerFactory = LogFactory.Create(output);
        }

        protected async Task<ClientServer> CreateClientServerAsync(byte unitIdentifier)
        {
            var clientServer = new ClientServer(1, loggerFactory);

            //Give the server (TcpListener) time to start up
            await Task.Delay(TimeSpan.FromSeconds(0.1));

            return clientServer;
        }

    }

    public class FunctionsShould : ClientServerTestBase
    {
        public FunctionsShould(ITestOutputHelper output) : base(output)
        {
        }

        [Theory]
        [InlineData(10, 42)]
        [InlineData(100, 420)]
        public async Task WriteSingleRegisterShouldWork(ushort address, ushort value)
        {
            await using var clientServer = await CreateClientServerAsync(1);

            await clientServer.Client.WriteSingleRegisterAsync(clientServer.UnitIdentifier, address, value);

            clientServer.Storage.HoldingRegisters[address].ShouldBe((ushort)value);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1000)]
        [InlineData(60000)]
        public async Task WriteMultipleRegistersShouldWork(ushort startingAddress)
        {
            await using var clientServer = await CreateClientServerAsync(1);

            await clientServer.Client.WriteMultipleRegistersAsync(clientServer.UnitIdentifier, startingAddress, new ushort[] { 1, 2, 3, 4, 5 });

            clientServer.Storage.HoldingRegisters[(ushort)(startingAddress + 0)].ShouldBe((ushort)1);
            clientServer.Storage.HoldingRegisters[(ushort)(startingAddress + 1)].ShouldBe((ushort)2);
            clientServer.Storage.HoldingRegisters[(ushort)(startingAddress + 2)].ShouldBe((ushort)3);
            clientServer.Storage.HoldingRegisters[(ushort)(startingAddress + 3)].ShouldBe((ushort)4);
            clientServer.Storage.HoldingRegisters[(ushort)(startingAddress + 4)].ShouldBe((ushort)5);
        }

        [Theory]
        [InlineData(100, new ushort[] { 4, 5, 6 })]
        [InlineData(1000, new ushort[] { 100, 2000, 7 })]
        [InlineData(60000, new ushort[] { 60, 0, 4, 5 })]
        public async Task ReadHoldingRegistersShouldWork(ushort startingAddress, ushort[] values)
        {
            await using var clientServer = await CreateClientServerAsync(1);

            for (int x = 0; x < values.Length; x++)
            {
                clientServer.Storage.HoldingRegisters[(ushort)(startingAddress + x)] = values[x];
            }
            
            var registers = await clientServer.Client.ReadHoldingRegistersAsync(clientServer.UnitIdentifier, startingAddress, (ushort)values.Length);

            registers.ShouldBe(values);
        }
    }
}
