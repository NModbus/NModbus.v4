using Microsoft.Extensions.Logging;
using NModbus.BasicServer.Interfaces;
using NModbus.Interfaces;

namespace NModbus.BasicServer
{
    public static class BasicModbusServerFactory
    {
        public static IModbusServer CreateBasicServer(
            byte unitNumber,
            ILoggerFactory loggerFactory,
            IDeviceStorage storage = null,
            IEnumerable<IServerFunction> customServerFunctions = null)
        {
            storage = storage ?? new PointStorage();

            //These are the built in function implementations.
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

            return new ModbusServer(unitNumber, dictionary.Values, loggerFactory.CreateLogger<ModbusServer>());
        }
    }
}