using Microsoft.Extensions.Logging;
using NModbus.Functions;
using NModbus.Interfaces;

namespace NModbus
{
    public class ModbusClient : IModbusClient
    {
        private readonly Dictionary<byte, IClientFunction> clientFunctions;
        private readonly ILogger logger;

        public ModbusClient(
            IEnumerable<IClientFunction> clientFunctions,
            IModbusTransport transport, 
            ILogger<ModbusClient> logger)
        {
            this.clientFunctions = clientFunctions
                .ToDictionary(f => f.FunctionCode);

            Transport = transport ?? throw new ArgumentNullException(nameof(transport));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public IModbusTransport Transport { get; }

        public bool TryGetClientFunction<TRequest, TResponse>(byte functionCode, out IClientFunction<TRequest, TResponse> clientFunction)
        {
            clientFunction = null;

            if (!clientFunctions.TryGetValue(functionCode, out var baseClientFunction))
                return false;

            clientFunction = baseClientFunction as IClientFunction<TRequest, TResponse>;

            return clientFunction != null;
        }

        public IClientFunction<TRequest, TResponse> GetClientFunction<TRequest, TResponse>(byte functionCode)
        {
            if (!TryGetClientFunction<TRequest, TResponse>(functionCode, out var clientFunction))
                throw new KeyNotFoundException($"Unable to find an {nameof(IClientFunction)}<{typeof(TRequest).Name},{typeof(TResponse).Name}> with function code 0x{functionCode:X2}");

            return clientFunction;
        }
    }
}
