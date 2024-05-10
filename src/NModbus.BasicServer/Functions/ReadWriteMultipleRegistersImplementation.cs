using Microsoft.Extensions.Logging;
using NModbus.BasicServer.Interfaces;
using NModbus.Interfaces;
using NModbus.Messages;

namespace NModbus.BasicServer.Functions
{
    public class ReadWriteMultipleRegistersImplementation : IModbusFunctionImplementation<ReadWriteMultipleRegistersRequest, ReadWriteMultipleRegistersResponse>
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly IDevicePointStorage<ushort> _storage;

        public ReadWriteMultipleRegistersImplementation(ILoggerFactory loggerFactory, IDevicePointStorage<ushort> storage)
        {
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
            _storage = storage ?? throw new ArgumentNullException(nameof(storage));
        }

        public Task<ReadWriteMultipleRegistersResponse> ProcessAsync(ReadWriteMultipleRegistersRequest request, CancellationToken cancellationToken)
        {
            _storage.WritePoints(request.WriteStartingAddress, request.WriteRegistersValue);

            var readRegisters = _storage.ReadPoints(request.ReadStartingAddress, request.QuantityToRead);

            return Task.FromResult(new ReadWriteMultipleRegistersResponse(readRegisters));
        }
    }
}
