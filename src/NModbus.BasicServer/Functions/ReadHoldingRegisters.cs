using Microsoft.Extensions.Logging;
using NModbus.BasicServer.Interfaces;
using NModbus.Interfaces;
using NModbus.Messages;

namespace NModbus.BasicServer.Functions
{
    public class ReadHoldingRegistersImplementation : IModbusFunctionImplementation<ReadHoldingRegistersRequest, ReadHoldingRegistersResponse>
    {
        private readonly ILogger logger;
        private readonly IDeviceStorage storage;

        public ReadHoldingRegistersImplementation(ILogger<ReadHoldingRegistersImplementation> logger, IDeviceStorage storage)
        {
            this.logger = logger;
            this.storage = storage;
        }

        public Task<ReadHoldingRegistersResponse> ProcessAsync(ReadHoldingRegistersRequest request, CancellationToken cancellationToken)
        {
            var registers = storage.HoldingRegisters.ReadPoints(request.StartingAddress, request.QuantityOfRegisters);

            return Task.FromResult(new ReadHoldingRegistersResponse
            {
                ByteCount = (byte)(registers.Length * 2),
                RegisterValues = registers
            });
        }
    }
}
