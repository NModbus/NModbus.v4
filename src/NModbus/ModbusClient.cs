using Microsoft.Extensions.Logging;
using NModbus.Extensions;
using NModbus.Functions;
using NModbus.Interfaces;
using NModbus.Messages;

namespace NModbus
{
    public class ModbusClient : IModbusClient
    {
        private readonly Dictionary<byte, IClientFunction> _clientFunctions;
        private readonly ILogger _logger;

        public ModbusClient(
            IModbusClientTransport transport,
            ILoggerFactory loggerFactory,
            IEnumerable<IClientFunction> customClientFunctions = null)
        {
            if (loggerFactory is null)
                throw new ArgumentNullException(nameof(loggerFactory));

            Transport = transport ?? throw new ArgumentNullException(nameof(transport));
            _logger = loggerFactory.CreateLogger<ModbusClient>();

            var defaultClientFunctions = new IClientFunction[]
            {
                new ModbusClientFunction<ReadCoilsRequest, ReadCoilsResponse>(ModbusFunctionCodes.ReadCoils, new ReadCoilsMessageSerializer()),
                new ModbusClientFunction<ReadDiscreteInputsRequest, ReadDiscreteInputsResponse>(ModbusFunctionCodes.ReadDiscreteInputs, new ReadDiscreteInputsMessageSerilizer()),
                new ModbusClientFunction<ReadHoldingRegistersRequest, ReadHoldingRegistersResponse>(ModbusFunctionCodes.ReadHoldingRegisters, new ReadHoldingRegistersMessageSerializer()),
                new ModbusClientFunction<ReadInputRegistersRequest, ReadInputRegistersResponse>(ModbusFunctionCodes.ReadInputRegisters, new ReadInputRegistersMessageSerializer()),
                new ModbusClientFunction<WriteSingleCoilRequest, WriteSingleCoilResponse>(ModbusFunctionCodes.WriteSingleCoil, new WriteSingleCoilMessageSerializer()),
                new ModbusClientFunction<WriteSingleRegisterRequest, WriteSingleRegisterResponse>(ModbusFunctionCodes.WriteSingleRegister, new WriteSingleRegisterMessageSerializer()),
                new ModbusClientFunction<WriteMultipleCoilsRequest, WriteMultipleCoilsResponse>(ModbusFunctionCodes.WriteMultipleCoils, new WriteMultipleCoilsMessageSerializer()),
                new ModbusClientFunction<WriteMultipleRegistersRequest, WriteMultipleRegistersResponse>(ModbusFunctionCodes.WriteMultipleRegisters, new WriteMultipleRegistersMessageSerializer()),
                new ModbusClientFunction<ReadFileRecordRequest, ReadFileRecordResponse>(ModbusFunctionCodes.ReadFileRecord, new ReadFileRecordMessageSerializer()),
                new ModbusClientFunction<WriteFileRecordRequest, WriteFileRecordResponse>(ModbusFunctionCodes.WriteFileRecord, new WriteFileRecordMessageSerializer()),
                new ModbusClientFunction<MaskWriteRegisterRequest, MaskWriteRegisterResponse>(ModbusFunctionCodes.MaskWriteRegister, new MaskWriteRegisterMessageSerializer()),
                new ModbusClientFunction<ReadWriteMultipleRegistersRequest, ReadWriteMultipleRegistersResponse>(ModbusFunctionCodes.ReadWriteMultipleRegisters, new ReadWriteMultipleRegistersMessageSerializer()),
                new ModbusClientFunction<ReadFifoQueueRequest, ReadFifoQueueResponse>(ModbusFunctionCodes.ReadFifoQueue, new ReadFifoQueueMessageSerializer()),
            };  

            _clientFunctions = defaultClientFunctions
                .ToDictionary(f => f.FunctionCode);

            //Now allow the caller to override any of the client functions (or add new ones).
            if (customClientFunctions != null)
            {
                foreach(var clientFunction in customClientFunctions)
                {
                    _logger.LogInformation("Custom implementation of function code {FunctionCode} with type {Type}.", $"0x{clientFunction.FunctionCode}", clientFunction.GetType().Name);
                    _clientFunctions[clientFunction.FunctionCode] = clientFunction;
                }
            }
        }

        public IModbusClientTransport Transport { get; }

        public virtual bool TryGetClientFunction<TRequest, TResponse>(byte functionCode, out IClientFunction<TRequest, TResponse> clientFunction)
        {
            clientFunction = null;

            if (!_clientFunctions.TryGetValue(functionCode, out var baseClientFunction))
            {
                _logger.LogWarning("Unable to find client function with function code {FunctionCode}", functionCode.ToHex());
                return false;
            }

            clientFunction = baseClientFunction as IClientFunction<TRequest, TResponse>;

            if (clientFunction == null)
            {
                _logger.LogWarning("A client function was found for function code {functionCode} but it was not of type " + nameof(IClientFunction) + "<{TRequest},{TResponse}>", functionCode, typeof(TRequest).Name, typeof(TResponse).Name);
            }

            return clientFunction != null;
        }
    }
}
