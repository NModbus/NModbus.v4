using NModbus.Interfaces;

namespace NModbus.Functions
{
    public class ModbusServerFunction<TRequest, TResponse> : IServerFunction
    {
        public byte FunctionCode { get; }

        public ModbusServerFunction(
            byte functionCode, 
            IModbusMessageSerializer<TRequest, TResponse> messageSerializer, 
            IModbusFunctionImplementation<TRequest, TResponse> implementation)
        {
            FunctionCode = functionCode;
            MessageSerializer = messageSerializer;
            Implementation = implementation;
        }

        /// <summary>
        /// The factory for serializing / deserialing the message.
        /// </summary>
        public IModbusMessageSerializer<TRequest, TResponse> MessageSerializer { get; }

        public IModbusFunctionImplementation<TRequest, TResponse> Implementation { get; }

        public virtual async Task<byte[]> ProcessAsync(byte[] data, CancellationToken cancellationToken)
        {
            //Get the request.
            var request = MessageSerializer.DeserializeRequest(data);

            //Process the request.
            var response = await Implementation.ProcessAsync(request, cancellationToken);

            //Get the response.
            return MessageSerializer.SerializeResponse(response);
        }
    }
}
