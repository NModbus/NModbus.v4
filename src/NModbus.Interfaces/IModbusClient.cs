using NModbus.Functions;

namespace NModbus.Interfaces
{
    /// <summary>
    /// Initiates communication and makes requests of server device.
    /// </summary>
    public interface IModbusClient
    {
        /// <summary>
        /// Attempt to retrieve a client function.
        /// </summary>
        /// <typeparam name="TRequest"></typeparam>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="functionCode"></param>
        /// <param name="clientFunction"></param>
        /// <returns></returns>
        bool TryGetClientFunction<TRequest, TResponse>(byte functionCode, out IClientFunction<TRequest, TResponse> clientFunction);

        IModbusClientTransport Transport { get; }
    }
}
