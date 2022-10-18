using Microsoft.Extensions.Logging;
using NModbus.BasicServer.Interfaces;
using NModbus.Interfaces;
using NModbus.Messages;

namespace NModbus.BasicServer.Functions
{
    public class WriteMultipleRegistersImplementation : IModbusFunctionImplementation<WriteMultipleRegistersRequest, WriteMultipleRegistersResponse>
    {
        private readonly ILogger logger;
        private readonly IDeviceStorage storage;

        public WriteMultipleRegistersImplementation(ILogger<WriteMultipleRegistersImplementation> logger, IDeviceStorage storage)
        {
            this.logger = logger;
            this.storage = storage;
        }

        public Task<WriteMultipleRegistersResponse> ProcessAsync(WriteMultipleRegistersRequest request, CancellationToken cancellationToken)
        {
            storage.HoldingRegisters.WritePoints(request.StartingAddress, request.Registers);

            return Task.FromResult(new WriteMultipleRegistersResponse(request.StartingAddress, (ushort)request.Registers.Length));
        }
    }
}
