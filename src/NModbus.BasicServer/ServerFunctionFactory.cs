using Microsoft.Extensions.Logging;
using NModbus.BasicServer.Functions;
using NModbus.BasicServer.Interfaces;
using NModbus.Functions;
using NModbus.Interfaces;
using NModbus.Messages;

namespace NModbus.BasicServer
{
    public static class ServerFunctionFactory
    {
        public static IServerFunction[] CreateBasicServerFunctions(
            ILoggerFactory loggerFactory,
            IDeviceStorage storage = null,
            IEnumerable<IServerFunction> customServerFunctions = null)
        {
            storage = storage ?? new Storage();

            //These are the built in function implementations.
            var serverFunctions = new IServerFunction[]
            {
                //Write Single Register
                new ModbusServerFunction<WriteSingleRegisterRequest, WriteSingleRegisterResponse>(
                    ModbusFunctionCodes.WriteSingleRegister,
                    new WriteSingleRegisterMessageSerializer(),
                    new WriteSingleRegisterImplementation(loggerFactory.CreateLogger<WriteSingleRegisterImplementation>(), storage)),

                //Read Holding Registers
                new ModbusServerFunction<ReadHoldingRegistersRequest, ReadHoldingRegistersResponse>(
                    ModbusFunctionCodes.ReadHoldingRegisters,
                    new ReadHoldingRegistersMessageSerializer(),
                    new ReadHoldingRegistersImplementation(loggerFactory.CreateLogger<ReadHoldingRegistersImplementation>(), storage)),
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

            return dictionary.Values
                .ToArray();
        }
    }
}