using Microsoft.Extensions.Logging;
using NModbus.BasicServer.Interfaces;
using NModbus.Interfaces;
using NModbus.Messages;

namespace NModbus.BasicServer.Functions
{
    public class ReadInputRegistersImplementation : IModbusFunctionImplementation<ReadInputRegistersRequest, ReadInputRegistersResponse>
    {
        private readonly ILoggerFactory loggerFactory;
        private readonly IDevicePointStorage<ushort> storage;

        public ReadInputRegistersImplementation(ILoggerFactory loggerFactory, IDevicePointStorage<ushort> storage)
        {
            this.loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
            this.storage = storage ?? throw new ArgumentNullException(nameof(storage));
        }

        public Task<ReadInputRegistersResponse> ProcessAsync(ReadInputRegistersRequest request, CancellationToken cancellationToken)
        {
            var points = storage.ReadPoints(request.StartingAddress, request.QuantityOfInputRegisters);

            return Task.FromResult(new ReadInputRegistersResponse(points));
        }
    }
}
