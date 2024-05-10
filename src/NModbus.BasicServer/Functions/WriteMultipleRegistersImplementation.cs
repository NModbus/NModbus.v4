using Microsoft.Extensions.Logging;
using NModbus.BasicServer.Interfaces;
using NModbus.Interfaces;
using NModbus.Messages;

namespace NModbus.BasicServer.Functions
{
    public class WriteMultipleRegistersImplementation : IModbusFunctionImplementation<WriteMultipleRegistersRequest, WriteMultipleRegistersResponse>
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly IDevicePointStorage<ushort> _storage;

        public WriteMultipleRegistersImplementation(ILoggerFactory loggerFactory, IDevicePointStorage<ushort> storage)
        {
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
            _storage = storage ?? throw new ArgumentNullException(nameof(storage));
        }

        public Task<WriteMultipleRegistersResponse> ProcessAsync(WriteMultipleRegistersRequest request, CancellationToken cancellationToken)
        {
            _storage.WritePoints(request.StartingAddress, request.Registers);

            return Task.FromResult(new WriteMultipleRegistersResponse(request.StartingAddress, (ushort)request.Registers.Length));
        }
    }
}
