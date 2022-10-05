using NModbus.Functions;
using NModbus.Interfaces;

namespace NModbus.Extensions
{
    public static class IModbusClientExtensions
    {
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
            var serializedRequest = await clientFunction.MessageSerializer.SerializeRequestAsync(request, cancellationToken);

            //Form the protocol data unit.
            var requestProtocolDataUnit = new ProtocolDataUnit(clientFunction.FunctionCode, serializedRequest);

            //Check to see if this is a broadcast request.
            if (unitNumber == 0)
            {
                //This is a broadcast request. No response is expected
                await client.Transport.SendAsync(unitNumber, requestProtocolDataUnit, cancellationToken);

                return default;
            }

            //Send the request and wait for a response.
            var responseProtocolDataUnit = await client.Transport.SendAndReceiveAsync(unitNumber, requestProtocolDataUnit, cancellationToken);

            //Check to see if this is an error response
            if (ModbusFunctionCodes.IsErrorBitSet(responseProtocolDataUnit.FunctionCode))
                throw new ModbusServerException((ModbusExceptionCode)responseProtocolDataUnit.Data.ToArray()[0]);

            //Deserialize the response.
            return await clientFunction.MessageSerializer.DeserializeResponseAsync(responseProtocolDataUnit.Data.ToArray());
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
    }
}
