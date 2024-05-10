using Microsoft.Extensions.Logging;
using NModbus.BasicServer.Interfaces;
using NModbus.Interfaces;
using NModbus.Messages;

namespace NModbus.BasicServer.Functions
{
    public class WriteSingleCoilImplementation : IModbusFunctionImplementation<WriteSingleCoilRequest, WriteSingleCoilResponse>
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly IDevicePointStorage<bool> _storage;

        public WriteSingleCoilImplementation(ILoggerFactory loggerFactory, IDevicePointStorage<bool> storage)
        {
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
            _storage = storage ?? throw new ArgumentNullException(nameof(storage));
        }

        public Task<WriteSingleCoilResponse> ProcessAsync(WriteSingleCoilRequest request, CancellationToken cancellationToken)
        {
            _storage.WritePoints(request.OutputAddress, new bool[] { request.OutputValue });

            return Task.FromResult(new WriteSingleCoilResponse(request.OutputAddress, request.OutputValue));
        }
    }
}
