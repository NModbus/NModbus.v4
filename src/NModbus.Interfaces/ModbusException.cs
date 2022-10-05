namespace NModbus.Interfaces
{
    public class ModbusServerException : Exception
    {
        public ModbusServerException(ModbusExceptionCode exceptionCode)
            : base($"Modbus Exception Code: {exceptionCode}")
        {
            ExceptionCode = exceptionCode;
        }

        public ModbusExceptionCode ExceptionCode { get; }
    }
}
