namespace NModbus.Functions
{
    public class ModbusClientFunction<TRequest, TResponse> : IClientFunction<TRequest, TResponse>
    {
        public ModbusClientFunction(byte functionCode, IModbusMessageSerializer<TRequest, TResponse> messageSerializer)
        {
            FunctionCode = functionCode;
            MessageSerializer = messageSerializer ?? throw new ArgumentNullException(nameof(messageSerializer));
        }

        public byte FunctionCode { get; }

        public IModbusMessageSerializer<TRequest, TResponse> MessageSerializer { get; }
    }
}
