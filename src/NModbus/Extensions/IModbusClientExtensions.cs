using NModbus.Functions;
using NModbus.Interfaces;
using NModbus.Messages;

namespace NModbus.Extensions
{
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
            byte unitNumber,
            TRequest request,
            CancellationToken cancellationToken = default)
        {
            //Find the client function.
            var clientFunction = client.GetClientFunction<TRequest, TResponse>(functionCode);

            //Serialize the request
            var serializedRequest = clientFunction.MessageSerializer.SerializeRequest(request);

            //Form the request
            var requestProtocolDataUnit = new ProtocolDataUnit(clientFunction.FunctionCode, serializedRequest);
            var requestApplicationDataUnit = new ApplicationDataUnit(unitNumber, requestProtocolDataUnit);

            //Check to see if this is a broadcast request.
            if (unitNumber == 0)
            {
                //This is a broadcast request. No response is expected
                await client.Transport.SendAsync(requestApplicationDataUnit, cancellationToken);

                return default;
            }

            //Send the request and wait for a response.
            var responseApplicationDataUnit = await client.Transport.SendAndReceiveAsync(requestApplicationDataUnit, cancellationToken);

            //Check to see if this is an error response
            if (ModbusFunctionCodes.IsErrorBitSet(responseApplicationDataUnit.ProtocolDataUnit.FunctionCode))
                throw new ModbusServerException((ModbusExceptionCode)responseApplicationDataUnit.ProtocolDataUnit.Data.ToArray()[0]);

            //Deserialize the response.
            return clientFunction.MessageSerializer.DeserializeResponse(responseApplicationDataUnit.ProtocolDataUnit.Data.ToArray());
        }

        public static async Task WriteSingleRegisterAsync(this IModbusClient client, byte unitNumber, ushort startingAddress, ushort value, CancellationToken cancellationToken = default)
        {
            var request = new WriteSingleRegisterRequest(startingAddress, value);

            await client.ExecuteAsync<WriteSingleRegisterRequest, WriteSingleRegisterResponse>(
                ModbusFunctionCodes.WriteSingleRegister,
                unitNumber,
                request,
                cancellationToken);
        }

        public static async Task<ushort[]> ReadHoldingRegistersAsync(this IModbusClient client, byte unitNumber, ushort startingAddress, ushort numberOfRegisters, CancellationToken cancellationToken = default)
        {
            var request = new ReadHoldingRegistersRequest(startingAddress, numberOfRegisters);

            var response = await client.ExecuteAsync<ReadHoldingRegistersRequest, ReadHoldingRegistersResponse>(
                ModbusFunctionCodes.ReadHoldingRegisters,
                unitNumber,
                request,
                cancellationToken);

            return response.RegisterValues;
        }

        public static async Task WriteMultipleRegistersAsync(this IModbusClient client, byte unitNumber, ushort startingAddress, ushort[] registers, CancellationToken cancellationToken = default)
        {
            var request = new WriteMultipleRegistersRequest(startingAddress, registers);

            await client.ExecuteAsync<WriteMultipleRegistersRequest, WriteMultipleRegistersResponse>(
                ModbusFunctionCodes.WriteMultipleRegisters,
                unitNumber,
                request,
                cancellationToken);
        }
    }
}
