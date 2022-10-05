using NModbus.Interfaces;

namespace NModbus.Functions
{
    public class ServerFunction<TRequest, TResponse> : IServerFunction
    {
        public byte FunctionCode { get; }

        public ServerFunction(
            byte functionCode, 
            IModbusMessageSerializer<TRequest, TResponse> messageFactory, 
            IModbusFunctionImplementation<TRequest, TResponse> implementation)
        {
            FunctionCode = functionCode;
            MessageFactory = messageFactory;
            Implementation = implementation;
        }

        /// <summary>
        /// The factory for serializing / deserialing the message.
        /// </summary>
        public IModbusMessageSerializer<TRequest, TResponse> MessageFactory { get; }

        public IModbusFunctionImplementation<TRequest, TResponse> Implementation { get; }

        public async Task<byte[]> ProcessAsync(byte[] data, CancellationToken cancellationToken)
        {
            //Get the request.
            var request = await MessageFactory.DeserializeRequestAsync(data, cancellationToken);

            //Process the request.
            var response = await Implementation.ProcessAsync(request, cancellationToken);

            //Get the response.
            return await MessageFactory.SerializeResponseAsync(response);
        }
    }
}
