using Microsoft.Extensions.Logging;
using NModbus.BasicServer.Interfaces;
using NModbus.Functions;
using NModbus.Interfaces;

namespace NModbus.BasicServer.Functions
{
    public class WriteSingleRegisterImplementation : IModbusFunctionImplementation<WriteSingleRegisterRequest, WriteSingleRegisterResponse>
    {
        private readonly ILogger<WriteSingleRegisterImplementation> logger;
        private readonly IBasicModbusStorage storage;

        public WriteSingleRegisterImplementation(ILogger<WriteSingleRegisterImplementation> logger, IBasicModbusStorage storage)
        {
            this.logger = logger;
            this.storage = storage;
        }

        public Task<WriteSingleRegisterResponse> ProcessAsync(WriteSingleRegisterRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
