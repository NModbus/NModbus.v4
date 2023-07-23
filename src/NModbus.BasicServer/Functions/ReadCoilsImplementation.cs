using Microsoft.Extensions.Logging;
using NModbus.BasicServer.Interfaces;
using NModbus.Interfaces;
using NModbus.Messages;

namespace NModbus.BasicServer.Functions
{
    public class ReadCoilsImplementation : IModbusFunctionImplementation<ReadCoilsRequest, ReadCoilsResponse>
    {
        private readonly IDevicePointStorage<bool> storage;

        public ReadCoilsImplementation(ILoggerFactory loggerFactory, IDevicePointStorage<bool> storage)
        {
            if (loggerFactory is null) throw new ArgumentNullException(nameof(loggerFactory));

            this.storage = storage ?? throw new ArgumentNullException(nameof(storage));
        }

        public Task<ReadCoilsResponse> ProcessAsync(ReadCoilsRequest request, CancellationToken cancellationToken)
        {
            var points = storage.ReadPoints(request.StartingAddress, request.QuantityOfOutputs);

            return Task.FromResult(new ReadCoilsResponse(points));
        }
    }
}
