using Microsoft.Extensions.Logging;
using NModbus.BasicServer.Interfaces;
using NModbus.Interfaces;
using NModbus.Messages;

namespace NModbus.BasicServer.Functions
{
    public class WriteSingleRegisterImplementation : IModbusFunctionImplementation<WriteSingleRegisterRequest, WriteSingleRegisterResponse>
    {
        private readonly ILoggerFactory loggerFactory;
        private readonly IDevicePointStorage<ushort> storage;

        public WriteSingleRegisterImplementation(ILoggerFactory loggerFactory, IDevicePointStorage<ushort> storage)
        {
            this.loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
            this.storage = storage ?? throw new ArgumentNullException(nameof(storage));
        }

        public Task<WriteSingleRegisterResponse> ProcessAsync(WriteSingleRegisterRequest request, CancellationToken cancellationToken)
        {
            storage.WritePoints(request.Address, new ushort[] { request.Value });

            return Task.FromResult(new WriteSingleRegisterResponse(request.Address, request.Value));
        }
    }
}
