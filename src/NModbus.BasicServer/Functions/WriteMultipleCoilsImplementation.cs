using Microsoft.Extensions.Logging;
using NModbus.BasicServer.Interfaces;
using NModbus.Interfaces;
using NModbus.Messages;

namespace NModbus.BasicServer.Functions
{
    public class WriteMultipleCoilsImplementation : IModbusFunctionImplementation<WriteMultipleCoilsRequest, WriteMultipleCoilsResponse>
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly IDevicePointStorage<bool> _storage;

        public WriteMultipleCoilsImplementation(ILoggerFactory loggerFactory, IDevicePointStorage<bool> storage)
        {
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
            _storage = storage ?? throw new ArgumentNullException(nameof(storage));
        }

        public Task<WriteMultipleCoilsResponse> ProcessAsync(WriteMultipleCoilsRequest request, CancellationToken cancellationToken)
        {
            _storage.WritePoints(request.StartingAddress, request.OutputsValue);

            return Task.FromResult(new WriteMultipleCoilsResponse(request.StartingAddress, (ushort)request.OutputsValue.Length));
        }
    }
}
