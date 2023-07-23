using Microsoft.Extensions.Logging;
using NModbus.BasicServer.Interfaces;
using NModbus.Helpers;
using NModbus.Interfaces;
using NModbus.Messages;

namespace NModbus.BasicServer.Functions
{
    public class ReadDiscreteInputsImplementation : IModbusFunctionImplementation<ReadDiscreteInputsRequest, ReadDiscreteInputsResponse>
    {
        private readonly IDevicePointStorage<bool> storage;

        public ReadDiscreteInputsImplementation(ILoggerFactory loggerFactory, IDevicePointStorage<bool> storage)
        {
            if (loggerFactory is null) throw new ArgumentNullException(nameof(loggerFactory));
            this.storage = storage ?? throw new ArgumentNullException(nameof(storage));
        }

        public Task<ReadDiscreteInputsResponse> ProcessAsync(ReadDiscreteInputsRequest request, CancellationToken cancellationToken)
        {
            var points = storage.ReadPoints(request.StartingAddress, request.QuantityOfInputs);

            return Task.FromResult(new ReadDiscreteInputsResponse(BitPacker.Pack(points)));
        }
    }
}
