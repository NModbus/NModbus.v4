using NModbus.BasicServer;
using NModbus.Extensions;
using Shouldly;

namespace NModbus.Tests.Integration
{
    public class FunctionsShould : ClientServerBase
    {
        [Fact]
        public async Task WriteSingleRegisterShouldWork()
        {
            await Client.WriteSingleRegisterAsync(UnitNumber, 10, 42);

            Storage.HoldingRegisters[10].ShouldBe((ushort)42);
        }

        [Fact]
        public async Task WriteMultipleRegistersShouldWork()
        {
            await Client.WriteMultipleRegistersAsync(UnitNumber, 10, new ushort[] { 1, 2, 3, 4, 5 });

            Storage.HoldingRegisters[10].ShouldBe((ushort)1);
            Storage.HoldingRegisters[11].ShouldBe((ushort)2);
            Storage.HoldingRegisters[12].ShouldBe((ushort)3);
            Storage.HoldingRegisters[13].ShouldBe((ushort)4);
            Storage.HoldingRegisters[14].ShouldBe((ushort)5);
        }

        [Fact]
        public async Task ReadHoldingRegistersShouldWork()
        {
            Storage.HoldingRegisters[100] = 4;
            Storage.HoldingRegisters[101] = 5;
            Storage.HoldingRegisters[102] = 6;

            var registers = await Client.ReadHoldingRegistersAsync(UnitNumber, 100, 3);

            registers.ShouldBe(new ushort[] { 4, 5, 6 });
        }

       
    }
}
