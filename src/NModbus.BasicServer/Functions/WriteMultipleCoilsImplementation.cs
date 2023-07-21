using Microsoft.Extensions.Logging;
using NModbus.BasicServer.Interfaces;
using NModbus.Interfaces;
using NModbus.Messages;

namespace NModbus.BasicServer.Functions
{
    public class WriteMultipleCoilsImplementation : IModbusFunctionImplementation<WriteMultipleCoilsRequest, WriteMultipleCoilsResponse>
    {
        private readonly ILoggerFactory loggerFactory;
        private readonly IDevicePointStorage<bool> storage;

        public WriteMultipleCoilsImplementation(ILoggerFactory loggerFactory, IDevicePointStorage<bool> storage)
        {
            this.loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
            this.storage = storage ?? throw new ArgumentNullException(nameof(storage));
        }

        public Task<WriteMultipleCoilsResponse> ProcessAsync(WriteMultipleCoilsRequest request, CancellationToken cancellationToken)
        {
            storage.WritePoints(request.StartingAddress, request.OutputsValue);

            return Task.FromResult(new WriteMultipleCoilsResponse(request.StartingAddress, (ushort)request.OutputsValue.Length));
        }
    }
}
