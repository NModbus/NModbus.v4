using Microsoft.Extensions.Logging;
using NModbus.BasicServer.Interfaces;
using NModbus.Interfaces;
using NModbus.Messages;

namespace NModbus.BasicServer.Functions
{
    public class WriteMultipleRegistersImplementation : IModbusFunctionImplementation<WriteMultipleRegistersRequest, WriteMultipleRegistersResponse>
    {
        private readonly ILoggerFactory loggerFactory;
        private readonly IDevicePointStorage<ushort> storage;

        public WriteMultipleRegistersImplementation(ILoggerFactory loggerFactory, IDevicePointStorage<ushort> storage)
        {
            this.loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
            this.storage = storage ?? throw new ArgumentNullException(nameof(storage));
        }

        public Task<WriteMultipleRegistersResponse> ProcessAsync(WriteMultipleRegistersRequest request, CancellationToken cancellationToken)
        {
            storage.WritePoints(request.StartingAddress, request.Registers);

            return Task.FromResult(new WriteMultipleRegistersResponse(request.StartingAddress, (ushort)request.Registers.Length));
        }
    }
}
