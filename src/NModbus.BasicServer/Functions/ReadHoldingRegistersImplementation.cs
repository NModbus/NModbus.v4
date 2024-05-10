using Microsoft.Extensions.Logging;
using NModbus.BasicServer.Interfaces;
using NModbus.Interfaces;
using NModbus.Messages;

namespace NModbus.BasicServer.Functions
{
    public class ReadHoldingRegistersImplementation : IModbusFunctionImplementation<ReadHoldingRegistersRequest, ReadHoldingRegistersResponse>
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly IDevicePointStorage<ushort> _storage;

        public ReadHoldingRegistersImplementation(ILoggerFactory loggerFactory, IDevicePointStorage<ushort> storage)
        {
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
            _storage = storage ?? throw new ArgumentNullException(nameof(storage));
        }

        public Task<ReadHoldingRegistersResponse> ProcessAsync(ReadHoldingRegistersRequest request, CancellationToken cancellationToken)
        {
            var registers = _storage.ReadPoints(request.StartingAddress, request.QuantityOfRegisters);

            return Task.FromResult(new ReadHoldingRegistersResponse(registers));
            
        }
    }
}
