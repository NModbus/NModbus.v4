using Microsoft.Extensions.Logging;
using NModbus.BasicServer.Interfaces;
using NModbus.Interfaces;
using NModbus.Messages;

namespace NModbus.BasicServer.Functions
{
    public class WriteSingleRegisterImplementation : IModbusFunctionImplementation<WriteSingleRegisterRequest, WriteSingleRegisterResponse>
    {
        private readonly ILogger logger;
        private readonly IDeviceStorage storage;

        public WriteSingleRegisterImplementation(ILogger<WriteSingleRegisterImplementation> logger, IDeviceStorage storage)
        {
            this.logger = logger;
            this.storage = storage;
        }

        public Task<WriteSingleRegisterResponse> ProcessAsync(WriteSingleRegisterRequest request, CancellationToken cancellationToken)
        {
            storage.HoldingRegisters.WritePoints(request.Address, new ushort[] { request.Value });

            return Task.FromResult(new WriteSingleRegisterResponse(request.Address, request.Value));
        }
    }
}
