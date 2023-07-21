using Microsoft.Extensions.Logging;
using NModbus.BasicServer.Interfaces;
using NModbus.Interfaces;
using NModbus.Messages;

namespace NModbus.BasicServer.Functions
{
    public class ReadWriteMultipleRegistersImplementation : IModbusFunctionImplementation<ReadWriteMultipleRegistersRequest, ReadWriteMultipleRegistersResponse>
    {
        private readonly ILoggerFactory loggerFactory;
        private readonly IDevicePointStorage<ushort> storage;

        public ReadWriteMultipleRegistersImplementation(ILoggerFactory loggerFactory, IDevicePointStorage<ushort> storage)
        {
            this.loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
            this.storage = storage ?? throw new ArgumentNullException(nameof(storage));
        }

        public Task<ReadWriteMultipleRegistersResponse> ProcessAsync(ReadWriteMultipleRegistersRequest request, CancellationToken cancellationToken)
        {
            storage.WritePoints(request.WriteStartingAddress, request.WriteRegistersValue);

            var readRegisters = storage.ReadPoints(request.ReadStartingAddress, request.QuantityToRead);

            return Task.FromResult(new ReadWriteMultipleRegistersResponse(readRegisters));
        }
    }
}
