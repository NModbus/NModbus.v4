using Microsoft.Extensions.Logging;
using NModbus.BasicServer.Interfaces;
using NModbus.Interfaces;
using NModbus.Messages;

namespace NModbus.BasicServer.Functions
{
    public class ReadHoldingRegistersImplementation : IModbusFunctionImplementation<ReadHoldingRegistersRequest, ReadHoldingRegistersResponse>
    {
        private readonly ILoggerFactory loggerFactory;
        private readonly IDevicePointStorage<ushort> storage;

        public ReadHoldingRegistersImplementation(ILoggerFactory loggerFactory, IDevicePointStorage<ushort> storage)
        {
            this.loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
            this.storage = storage ?? throw new ArgumentNullException(nameof(storage));
        }

        public Task<ReadHoldingRegistersResponse> ProcessAsync(ReadHoldingRegistersRequest request, CancellationToken cancellationToken)
        {
            var registers = storage.ReadPoints(request.StartingAddress, request.QuantityOfRegisters);

            return Task.FromResult(new ReadHoldingRegistersResponse(registers));
            
        }
    }
}
