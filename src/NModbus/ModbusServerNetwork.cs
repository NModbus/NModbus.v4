using Microsoft.Extensions.Logging;
using NModbus.Interfaces;
using System.Collections.Concurrent;

namespace NModbus
{
    public class ModbusServerNetwork : IModbusServerNetwork
    {
        private readonly ConcurrentDictionary<byte, IModbusServer> servers = new ConcurrentDictionary<byte, IModbusServer>();
        private readonly ILogger logger;
        private readonly ILoggerFactory loggerFactory;

        public ModbusServerNetwork(ILoggerFactory loggerFactory)
        {
            this.loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
            this.logger = loggerFactory.CreateLogger<ModbusServerNetwork>();
        }

        public bool TryAddServer(IModbusServer server)
        {
            return servers.TryAdd(server.UnitIdentifier, server);
        }

        public bool TryRemoveServer(byte unitNumnber)
        {
            return servers.TryRemove(unitNumnber, out _);
        }

        public async Task ProcessRequestAsync(
            IModbusMessage requestMessage, 
            IModbusClientTransport clientTransport, 
            CancellationToken cancellationToken = default)
        {
            if (requestMessage.UnitIdentifier == 0)
            {
                foreach (var server in servers.Values)
                {
                    await server.ProcessRequestAsync(requestMessage.ProtocolDataUnit, cancellationToken);
                }
            }
            else
            {
                if (servers.TryGetValue(requestMessage.UnitIdentifier, out var server))
                {
                    var response = await server.ProcessRequestAsync(requestMessage.ProtocolDataUnit, cancellationToken);

                    if (response != null)
                    {
                        await clientTransport.SendAsync(new ModbusMessage(requestMessage.UnitIdentifier, response), cancellationToken);
                    }
                }
                else
                {
                    //TODO: Send an exception message that the unit wasn't found. Or do we just timeout. Hmm. Look in the docs.
                }
            }
        }
    }
}
