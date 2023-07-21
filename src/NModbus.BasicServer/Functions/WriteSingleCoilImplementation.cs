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
}
