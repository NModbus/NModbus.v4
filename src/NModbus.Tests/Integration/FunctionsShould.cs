using NModbus.BasicServer;
using NModbus.Extensions;
using Shouldly;
using Xunit.Abstractions;

namespace NModbus.Tests.Integration
{
    public class FunctionsShould : ClientServerBase
    {
        public FunctionsShould(ITestOutputHelper  output)
        {
            output.WriteLine("Constructor");
        }

        [Theory]
        [InlineData(10, 42)]
        [InlineData(100, 420)]
        public async Task WriteSingleRegisterShouldWork(ushort address, ushort value)
        {
            await Client.WriteSingleRegisterAsync(UnitNumber, address, value);

            Storage.HoldingRegisters[address].ShouldBe((ushort)value);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1000)]
        [InlineData(60000)]
        public async Task WriteMultipleRegistersShouldWork(ushort startingAddress)
        {
            await Client.WriteMultipleRegistersAsync(UnitNumber, startingAddress, new ushort[] { 1, 2, 3, 4, 5 });

            Storage.HoldingRegisters[(ushort)(startingAddress + 0)].ShouldBe((ushort)1);
            Storage.HoldingRegisters[(ushort)(startingAddress + 1)].ShouldBe((ushort)2);
            Storage.HoldingRegisters[(ushort)(startingAddress + 2)].ShouldBe((ushort)3);
            Storage.HoldingRegisters[(ushort)(startingAddress + 3)].ShouldBe((ushort)4);
            Storage.HoldingRegisters[(ushort)(startingAddress + 4)].ShouldBe((ushort)5);
        }

        [Theory]
        [InlineData(100)]
        [InlineData(1000)]
        [InlineData(60000)]
        public async Task ReadHoldingRegistersShouldWork(ushort startingAddress)
        {
            const int numberOfRegisters = 3;

            Storage.HoldingRegisters[(ushort)(startingAddress + 0)] = 4;
            Storage.HoldingRegisters[(ushort)(startingAddress + 1)] = 5;
            Storage.HoldingRegisters[(ushort)(startingAddress + 2)] = 6;

            var registers = await Client.ReadHoldingRegistersAsync(UnitNumber, startingAddress, numberOfRegisters);

            registers.ShouldBe(new ushort[] { 4, 5, 6 });
        }
    }
}
