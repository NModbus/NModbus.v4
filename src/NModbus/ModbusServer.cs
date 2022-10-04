using Microsoft.Extensions.Logging;
using NModbus.Interfaces;

namespace NModbus
{
    public class ModbusServer : IModbusServer
    {
        private readonly IModbusTransport transport;
        private readonly IModbusServerHandler handler;
        private readonly ILogger<ModbusServer> logger;

        public ModbusServer(
            IModbusTransport transport, 
            IModbusServerHandler handler, 
            ILogger<ModbusServer> logger)
        {
            this.transport = transport ?? throw new ArgumentNullException(nameof(transport));
            this.handler = handler ?? throw new ArgumentNullException(nameof(handler));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Task ListenAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
