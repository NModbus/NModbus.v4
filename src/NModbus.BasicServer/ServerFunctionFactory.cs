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
            loggerFactory ??= new NullLoggerFactory();
            storage ??= new Storage();

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

                //Read Write Multiple Registers
                new ModbusServerFunction<ReadWriteMultipleRegistersRequest, ReadWriteMultipleRegistersResponse>(
                    ModbusFunctionCodes.ReadWriteMultipleRegisters,
                    new ReadWriteMultipleRegistersMessageSerializer(),
                    new ReadWriteMultipleRegistersImplementation(loggerFactory, storage.HoldingRegisters)),

                //Read Input Registers
                new ModbusServerFunction<ReadInputRegistersRequest, ReadInputRegistersResponse>(
                    ModbusFunctionCodes.ReadInputRegisters,
                    new ReadInputRegistersMessageSerializer(),
                    new ReadInputRegistersImplementation(loggerFactory, storage.InputRegisters)),

                //Write Single Coil
                new ModbusServerFunction<WriteSingleCoilRequest, WriteSingleCoilResponse>(
                    ModbusFunctionCodes.WriteSingleCoil,
                    new WriteSingleCoilMessageSerializer(),
                    new WriteSingleCoilImplementation(loggerFactory, storage.Coils)),

                //Write Multiple Coils
                new ModbusServerFunction<WriteMultipleCoilsRequest, WriteMultipleCoilsResponse>(
                    ModbusFunctionCodes.WriteMultipleCoils,
                    new WriteMultipleCoilsMessageSerializer(),
                    new WriteMultipleCoilsImplementation(loggerFactory,storage.Coils)),

                //Read Coils
                new ModbusServerFunction<ReadCoilsRequest, ReadCoilsResponse>(
                    ModbusFunctionCodes.ReadCoils,
                    new ReadCoilsMessageSerializer(),
                    new ReadCoilsImplementation(loggerFactory, storage.Coils)),

                //Read Discrete Inputs
                new ModbusServerFunction<ReadDiscreteInputsRequest, ReadDiscreteInputsResponse>(
                    ModbusFunctionCodes.ReadDiscreteInputs,
                    new ReadDiscreteInputsMessageSerilizer(),
                    new ReadDiscreteInputsImplementation(loggerFactory, storage.DiscreteInputs)),

                //Mask Write Register
                new ModbusServerFunction<MaskWriteRegisterRequest, MaskWriteRegisterResponse>(
                    ModbusFunctionCodes.MaskWriteRegister,
                    new MaskWriteRegisterMessageSerializer(),
                    new MaskWriteRegisterImplementation(loggerFactory, storage.HoldingRegisters)),
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