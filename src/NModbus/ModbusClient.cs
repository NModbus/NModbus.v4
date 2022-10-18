using Microsoft.Extensions.Logging;
using NModbus.Extensions;
using NModbus.Functions;
using NModbus.Interfaces;
using NModbus.Messages;

namespace NModbus
{
    public class ModbusClient : IModbusClient
    {
        private readonly Dictionary<byte, IClientFunction> clientFunctions;
        private readonly ILogger logger;

        public ModbusClient(
            IModbusTransport transport,
            ILogger<ModbusClient> logger,
            IEnumerable<IClientFunction> customClientFunctions = null)
        {
            Transport = transport ?? throw new ArgumentNullException(nameof(transport));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));

            var defaultClientFunctions = new IClientFunction[]
            {
                new ModbusClientFunction<WriteSingleRegisterRequest, WriteSingleRegisterResponse>(ModbusFunctionCodes.WriteSingleRegister, new WriteSingleRegisterMessageSerializer()),
                new ModbusClientFunction<ReadHoldingRegistersRequest, ReadHoldingRegistersResponse>(ModbusFunctionCodes.ReadHoldingRegisters, new ReadHoldingRegistersMessageSerializer()),
                new ModbusClientFunction<WriteMultipleRegistersRequest, WriteMultipleRegistersResponse>(ModbusFunctionCodes.WriteMultipleRegisters, new WriteMultipleRegistersMessageSerializer())
            };

            clientFunctions = defaultClientFunctions
                .ToDictionary(f => f.FunctionCode);

            //Now allow the caller to override any of the client functions (or add new ones).
            if (customClientFunctions != null)
            {
                foreach(var clientFunction in customClientFunctions)
                {
                    logger.LogInformation("Custom implementation of function code {FunctionCode} with type {Type}.", $"0x{clientFunction.FunctionCode}", clientFunction.GetType().Name);
                    clientFunctions[clientFunction.FunctionCode] = clientFunction;
                }
            }
        }

        public IModbusTransport Transport { get; }

        public virtual bool TryGetClientFunction<TRequest, TResponse>(byte functionCode, out IClientFunction<TRequest, TResponse> clientFunction)
        {
            clientFunction = null;

            if (!clientFunctions.TryGetValue(functionCode, out var baseClientFunction))
            {
                logger.LogWarning("Unable to find client function with function code {FunctionCode}", functionCode.ToHex());
                return false;
            }

            clientFunction = baseClientFunction as IClientFunction<TRequest, TResponse>;

            if (clientFunction == null)
            {
                logger.LogWarning("A client function was found for function code {functionCode} but it was not of type " + nameof(IClientFunction) + "<{TRequest},{TResponse}>", functionCode, typeof(TRequest).Name, typeof(TResponse).Name);
            }

            return clientFunction != null;
        }
    }
}
