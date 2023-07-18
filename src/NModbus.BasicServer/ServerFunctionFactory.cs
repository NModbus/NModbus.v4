using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
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
            IDeviceStorage storage = null,
            ILoggerFactory loggerFactory = null,
            IEnumerable<IServerFunction> customServerFunctions = null)
        {
            loggerFactory = loggerFactory ?? new NullLoggerFactory();
            storage = storage ?? new Storage();

            //These are the built in function implementations.
            var serverFunctions = new IServerFunction[]
            {
                //Write Single Register
                new ModbusServerFunction<WriteSingleRegisterRequest, WriteSingleRegisterResponse>(
                    ModbusFunctionCodes.WriteSingleRegister,
                    new WriteSingleRegisterMessageSerializer(),
                    new WriteSingleRegisterImplementation(loggerFactory, storage.HoldingRegisters)),

                //Read Holding Registers
                new ModbusServerFunction<ReadHoldingRegistersRequest, ReadHoldingRegistersResponse>(
                    ModbusFunctionCodes.ReadHoldingRegisters,
                    new ReadHoldingRegistersMessageSerializer(),
                    new ReadHoldingRegistersImplementation(loggerFactory, storage.HoldingRegisters)),

                //Write Multiple Registers
                new ModbusServerFunction<WriteMultipleRegistersRequest, WriteMultipleRegistersResponse>(
                    ModbusFunctionCodes.WriteMultipleRegisters,
                    new WriteMultipleRegistersMessageSerializer(),
                    new WriteMultipleRegistersImplementation(loggerFactory, storage.HoldingRegisters)),

                //Read Input Registers
                new ModbusServerFunction<ReadInputRegistersRequest, ReadInputRegistersResponse>(
                    ModbusFunctionCodes.ReadInputRegisters,
                    new ReadInputRegistersMessageSerializer(),
                    new ReadInputRegistersImplementation(loggerFactory, storage.InputRegisters))
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