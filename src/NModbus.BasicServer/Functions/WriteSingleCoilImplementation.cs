using Microsoft.Extensions.Logging;
using NModbus.BasicServer.Interfaces;
using NModbus.Interfaces;
using NModbus.Messages;

namespace NModbus.BasicServer.Functions
{
    public class WriteSingleCoilImplementation : IModbusFunctionImplementation<WriteSingleCoilRequest, WriteSingleCoilResponse>
    {
        private readonly ILoggerFactory loggerFactory;
        private readonly IDevicePointStorage<bool> storage;

        public WriteSingleCoilImplementation(ILoggerFactory loggerFactory, IDevicePointStorage<bool> storage)
        {
            this.loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
            this.storage = storage ?? throw new ArgumentNullException(nameof(storage));
        }

        public Task<WriteSingleCoilResponse> ProcessAsync(WriteSingleCoilRequest request, CancellationToken cancellationToken)
        {
            storage.WritePoints(request.OutputAddress, new bool[] { request.OutputValue });

            return Task.FromResult(new WriteSingleCoilResponse(request.OutputAddress, request.OutputValue));
        }
    }

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
