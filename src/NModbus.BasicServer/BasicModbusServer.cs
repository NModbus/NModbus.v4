using Microsoft.Extensions.Logging;
using NModbus.BasicServer.Interfaces;
using NModbus.Interfaces;

namespace NModbus.BasicServer
{
    public static class BasicModbusServerFactory
    {
        public static IModbusServer CreateBasicServer(
            IModbusTransport transport, 
            ILoggerFactory loggerFactory,
            IBasicModbusStorage storage = null,
            IEnumerable<IServerFunction> customServerFunctions = null)
        {
            storage = storage ?? new BasicModbusStorage();

            var serverFunctions = new IServerFunction[]
            {

            };

            var dictionary = serverFunctions
                .ToDictionary(f => f.FunctionCode);

            if (customServerFunctions != null)
            {
                foreach(var customServerFunction in customServerFunctions)
                {
                    dictionary[customServerFunction.FunctionCode] = customServerFunction;
                }
            }

            return new ModbusServer(dictionary.Values, loggerFactory.CreateLogger<ModbusServer>());
        }
    }
}