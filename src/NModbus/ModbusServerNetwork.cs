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
            return servers.TryAdd(server.UnitNumber, server);
        }

        public bool TryRemoveServer(byte unitNumnber)
        {
            return servers.TryRemove(unitNumnber, out _);
        }

        public async Task ProcessRequestAsync(ApplicationDataUnit applicationDataUnit, IModbusClientTransport clientTransport, CancellationToken cancellationToken = default)
        {
            if (applicationDataUnit.UnitNumber == 0)
            {
                foreach (var server in servers.Values)
                {
                    await server.ProcessRequestAsync(applicationDataUnit.ProtocolDataUnit, cancellationToken);
                }
            }
            else
            {
                if (servers.TryGetValue(applicationDataUnit.UnitNumber, out var server))
                {
                    var response = await server.ProcessRequestAsync(applicationDataUnit.ProtocolDataUnit, cancellationToken);

                    if (response != null)
                    {
                        await clientTransport.SendAsync(new ApplicationDataUnit(applicationDataUnit.UnitNumber, response), cancellationToken);
                    }
                }
            }
        }
    }
}
