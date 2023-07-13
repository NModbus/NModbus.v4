using NModbus.Functions;
using NModbus.Interfaces;
using NModbus.Messages;

namespace NModbus
{
    /// <summary>
    /// Convenient methods for calling Modbus functions on <see cref="IModbusClient"/>.
    /// </summary>
    public static class IModbusClientExtensions
    {
        /// <summary>
        /// Throws an exception of the specified function isn't available.
        /// </summary>
        /// <typeparam name="TRequest"></typeparam>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="client"></param>
        /// <param name="functionCode"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public static IClientFunction<TRequest, TResponse> GetClientFunction<TRequest, TResponse>(this IModbusClient client, byte functionCode)
        {
            if (!client.TryGetClientFunction<TRequest, TResponse>(functionCode, out var clientFunction))
                throw new KeyNotFoundException($"Unable to find an {nameof(IClientFunction)}<{typeof(TRequest).Name},{typeof(TResponse).Name}> with function code 0x{functionCode:X2}");

            return clientFunction;
        }

        public static async Task<TResponse> ExecuteAsync<TRequest, TResponse>(
            this IModbusClient client,
            byte functionCode,
            byte unitIdentifier,
            TRequest request,
            CancellationToken cancellationToken = default)
        {
            //Find the client function.
            var clientFunction = client.GetClientFunction<TRequest, TResponse>(functionCode);

            //Serialize the request
            var serializedRequest = clientFunction.MessageSerializer.SerializeRequest(request);

            //Form the request
            var requestProtocolDataUnit = new ProtocolDataUnit(clientFunction.FunctionCode, serializedRequest);
            var requestMessage = new ModbusMessage(unitIdentifier, requestProtocolDataUnit);

            //Check to see if this is a broadcast request.
            if (unitIdentifier == 0)
            {
                //This is a broadcast request. No response is expected
                await client.Transport.SendAsync(requestMessage, cancellationToken);

                return default;
            }

            //Send the request and wait for a response.
            var responseMessage = await client.Transport.SendAndReceiveAsync(requestMessage, cancellationToken);

            //Check to see if this is an error response
            if (ModbusFunctionCodes.IsErrorBitSet(responseMessage.ProtocolDataUnit.FunctionCode))
                throw new ModbusServerException((ModbusExceptionCode)responseMessage.ProtocolDataUnit.Data.ToArray()[0]);

            //Deserialize the response.
            return clientFunction.MessageSerializer.DeserializeResponse(responseMessage.ProtocolDataUnit.Data.ToArray());
        }

        public static async Task<bool[]> ReadCoilsAsync(
            this IModbusClient client, 
            byte unitIdentifier, 
            ushort startingAddress, 
            ushort quantityOfOutputs,
            CancellationToken cancellationToken = default)
        {
            var request = new ReadCoilsRequest(startingAddress, quantityOfOutputs);

            var response = await client.ExecuteAsync<ReadCoilsRequest, ReadCoilsResponse>(
                ModbusFunctionCodes.ReadCoils, 
                unitIdentifier, 
                request, 
                cancellationToken);

            //TODO: parse out the correct number of outputs according to the request.
            throw new NotImplementedException();
        }

        public static async Task<bool[]> ReadDiscreteInputsAsync(
            this IModbusClient client,
            byte unitIdentifier,
            ushort startingAddress,
            ushort quantityOfInputs,
            CancellationToken cancellationToken = default)
        {
            var request = new ReadDiscreteInputsRequest(startingAddress, quantityOfInputs);

            var response = await client.ExecuteAsync<ReadDiscreteInputsRequest, ReadDiscreteInputsResponse>(
                ModbusFunctionCodes.ReadDiscreteInputs, 
                unitIdentifier, 
                request, 
                cancellationToken);

            //TODO: parse out the correct number of inputs according to the request.
            throw new NotImplementedException();
        }

        public static async Task<ushort[]> ReadHoldingRegistersAsync(this IModbusClient client, byte unitIdentifier, ushort startingAddress, ushort numberOfRegisters, CancellationToken cancellationToken = default)
        {
            var request = new ReadHoldingRegistersRequest(startingAddress, numberOfRegisters);

            var response = await client.ExecuteAsync<ReadHoldingRegistersRequest, ReadHoldingRegistersResponse>(
                ModbusFunctionCodes.ReadHoldingRegisters,
                unitIdentifier,
                request,
                cancellationToken);

            return response.RegisterValues;
        }

        public static async Task WriteSingleRegisterAsync(this IModbusClient client, byte unitIdentifier, ushort startingAddress, ushort value, CancellationToken cancellationToken = default)
        {
            var request = new WriteSingleRegisterRequest(startingAddress, value);

            await client.ExecuteAsync<WriteSingleRegisterRequest, WriteSingleRegisterResponse>(
                ModbusFunctionCodes.WriteSingleRegister,
                unitIdentifier,
                request,
                cancellationToken);
        }

        public static async Task WriteMultipleCoils(
            this IModbusClient client, 
            byte unitIdentifier, 
            ushort startingAddress, 
            bool[] outputsValue, 
            CancellationToken cancellationToken = default)
        {
            var request = new WriteMultipleCoilsRequest(startingAddress, outputsValue);

            await client.ExecuteAsync<WriteMultipleCoilsRequest, WriteMultipleCoilsResponse>(
                ModbusFunctionCodes.WriteMultipleCoils,
                unitIdentifier,
                request,
                cancellationToken);
        }

        public static async Task WriteMultipleRegistersAsync(this IModbusClient client, byte unitIdentifier, ushort startingAddress, ushort[] registers, CancellationToken cancellationToken = default)
        {
            var request = new WriteMultipleRegistersRequest(startingAddress, registers);

            await client.ExecuteAsync<WriteMultipleRegistersRequest, WriteMultipleRegistersResponse>(
                ModbusFunctionCodes.WriteMultipleRegisters,
                unitIdentifier,
                request,
                cancellationToken);
        }

        //TODO: ReadFileRecord
        //TODO: WriteFileRecord

        public static async Task MaskWriteRegisterAsync(
            this IModbusClient client,
            byte unitIdentifier,
            ushort referenceAddress,
            ushort andMask,
            ushort orMask,
            CancellationToken cancellationToken = default)
        {
            var request = new MaskWriteRegisterRequest(referenceAddress, andMask, orMask);

            await client.ExecuteAsync<MaskWriteRegisterRequest, MaskWriteRegisterResponse>(
                ModbusFunctionCodes.MaskWriteRegister,
                unitIdentifier,
                request,
                cancellationToken);
        }

        public static async Task<ushort[]> ReadWriteMultipleRegisters(
            this IModbusClient client,
            byte unitIdentifier,
            ushort readStartingAddress,
            ushort quantityToRead,
            ushort writeStartingAddress,
            ushort[] writeRegistersValue,
            CancellationToken cancellationToken = default)
        {
            var request = new ReadWriteMultipleRegistersRequest(
                readStartingAddress,
                quantityToRead,
                writeStartingAddress,
                writeRegistersValue);

            var response = await client.ExecuteAsync<ReadWriteMultipleRegistersRequest, ReadWriteMultipleRegistersResponse>(
                ModbusFunctionCodes.ReadWriteMultipleRegisters,
                unitIdentifier,
                request,
                cancellationToken);

            return response.ReadRegistersValue;
        }

        public static async Task<ushort[]> ReadFifoQueueAsync(
            this IModbusClient client,
            byte unitIdentifier,
            ushort fifoPointerAddress,
            CancellationToken cancellationToken = default)
        {
            var request = new ReadFifoQueueRequest(fifoPointerAddress);

            var response = await client.ExecuteAsync<ReadFifoQueueRequest, ReadFifoQueueResponse>(
                ModbusFunctionCodes.ReadFifoQueue,
                unitIdentifier,
                request,
                cancellationToken);

            return response.FifoValueRegister;
        }
    }
}
