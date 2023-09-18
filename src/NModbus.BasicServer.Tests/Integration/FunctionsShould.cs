using Xunit.Abstractions;

namespace NModbus.BasicServer.Tests.Integration
{
    public class FunctionsShould : ClientServerTestBase
    {
        public FunctionsShould(ITestOutputHelper output) : base(output)
        {
        }

        [Theory]
        [InlineData(10, true)]
        [InlineData(100, false)]
        [InlineData(1000, true)]
        public async Task WriteSingleCoilShouldWork(ushort address, bool value)
        {
            await using var clientServer = await CreateClientServerAsync(1);

            await clientServer.Client.WriteSingleCoilAsync(clientServer.UnitIdentifier, address, value);

            clientServer.Storage.Coils[address].ShouldBe(value);
        }

        [Theory]
        [InlineData(10, 42)]
        [InlineData(100, 420)]
        public async Task WriteSingleRegisterShouldWork(ushort address, ushort value)
        {
            await using var clientServer = await CreateClientServerAsync(1);

            await clientServer.Client.WriteSingleRegisterAsync(clientServer.UnitIdentifier, address, value);

            clientServer.Storage.HoldingRegisters[address].ShouldBe(value);
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

        [Theory]
        [InlineData(100, new ushort[] { 4, 5, 6 })]
        [InlineData(1000, new ushort[] { 100, 2000, 7 })]
        [InlineData(60000, new ushort[] { 60, 0, 4, 5 })]
        public async Task ReadInputRegistersShouldWork(ushort startingAddress, ushort[] values)
        {
            await using var clientServer = await CreateClientServerAsync(1);

            for (int x = 0; x < values.Length; x++)
            {
                clientServer.Storage.InputRegisters[(ushort)(startingAddress + x)] = values[x];
            }

            var registers = await clientServer.Client.ReadInputRegistersAsync(clientServer.UnitIdentifier, startingAddress, (ushort)values.Length);

            registers.ShouldBe(values);
        }
    }
}
