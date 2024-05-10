using Microsoft.Extensions.Logging;
using NModbus.BasicServer.Interfaces;
using NModbus.Interfaces;
using NModbus.Messages;

namespace NModbus.BasicServer.Functions
{
    public class ReadInputRegistersImplementation : IModbusFunctionImplementation<ReadInputRegistersRequest, ReadInputRegistersResponse>
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly IDevicePointStorage<ushort> _storage;

        public ReadInputRegistersImplementation(ILoggerFactory loggerFactory, IDevicePointStorage<ushort> storage)
        {
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
            _storage = storage ?? throw new ArgumentNullException(nameof(storage));
        }

        public Task<ReadInputRegistersResponse> ProcessAsync(ReadInputRegistersRequest request, CancellationToken cancellationToken)
        {
            var points = _storage.ReadPoints(request.StartingAddress, request.QuantityOfInputRegisters);

            return Task.FromResult(new ReadInputRegistersResponse(points));
        }
    }
}
