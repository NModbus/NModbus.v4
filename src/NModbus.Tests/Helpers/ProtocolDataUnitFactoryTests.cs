using NModbus.Helpers;
using NModbus.Interfaces;

namespace NModbus.Tests.Helpers
{
    public class ProtocolDataUnitFactoryTests
    {
        [Theory]
        [InlineData(ModbusFunctionCodes.ReadHoldingRegisters, ModbusExceptionCode.IllegalDataAddress, new byte[] { 0b10000000 | 0x03, (byte)ModbusExceptionCode.IllegalDataAddress  })]
        public void CreateException_ShouldWork(byte functionCode, ModbusExceptionCode exceptionCode, byte[] expectedBytes)
        {
            var protocolDataUnit = ProtocolDataUnitFactory.CreateException(functionCode, exceptionCode);

            protocolDataUnit.ToArray()
                .ShouldBe(expectedBytes);
        }
    }
}
