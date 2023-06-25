using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NModbus.Helpers;
using NModbus.Interfaces;

namespace NModbus
{
    public class ModbusServer : IModbusServer
    {
        private readonly Dictionary<byte, IServerFunction> serverFunctions;
        protected readonly ILogger<ModbusServer> logger;

        public ModbusServer(
            byte unitNumber,
            IEnumerable<IServerFunction> serverFunctions,
            ILoggerFactory loggerFactory)
        {
            this.serverFunctions = serverFunctions.ToDictionary(f => f.FunctionCode);
            UnitNumber = unitNumber;
            logger = loggerFactory.CreateLogger<ModbusServer>();
        }

        /// <summary>
        /// Gets the unit number of the server device.
        /// </summary>
        public byte UnitNumber { get; }

        public async virtual Task<ProtocolDataUnit> ProcessRequestAsync(ProtocolDataUnit request, CancellationToken cancellationToken)
        {
            //Try to find the function for this request
            if (!serverFunctions.TryGetValue(request.FunctionCode, out var serverFunction))
            {
                //This function code isn't supported.
                return ProtocolDataUnitFactory.CreateException(request.FunctionCode, ModbusExceptionCode.IllegalFunction);
            }

            try
            {
                //Attempt to process the request.
                var responseData = await serverFunction.ProcessAsync(request.Data.ToArray(), cancellationToken);

                //Return the result
                return new ProtocolDataUnit(request.FunctionCode, responseData);
            }
            catch (ModbusServerException exception)
            {
                logger.LogError(exception, "A Modbus error {ExceptionCode} occurred while processing function 0x{FunctionCode:X2}", exception.ExceptionCode, request.FunctionCode);

                //Create a message that passes on the exception code that was specified in the exception.
                return ProtocolDataUnitFactory.CreateException(request.FunctionCode, exception.ExceptionCode);
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "An error occurred while processing function 0x{FunctionCode:X2}", request.FunctionCode);

                //We're not sure what happened here, so just return a catastrohpic error.
                return ProtocolDataUnitFactory.CreateException(request.FunctionCode, ModbusExceptionCode.ServerDeviceFailure);
            }
        }
    }
}
