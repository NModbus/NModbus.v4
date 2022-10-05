using NModbus.Interfaces;

namespace NModbus.Helpers
{
    public static class ProtocolDataUnitFactory
    {
        /// <summary>
        /// Creates a Modbus Exception <see cref="ProtocolDataUnit"/>.
        /// </summary>
        /// <param name="functionCode"></param>
        /// <param name="exceptionCode"></param>
        /// <returns></returns>
        public static ProtocolDataUnit CreateException(byte functionCode, ModbusExceptionCode exceptionCode)
        {
            return new ProtocolDataUnit(ModbusFunctionCodes.SetErrorBit(functionCode), new byte[]
                {
                    (byte)ModbusExceptionCode.IllegalFunction
                });
        }
    }
}
