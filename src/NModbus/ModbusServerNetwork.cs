using NModbus.Interfaces;

namespace NModbus
{
    public class ModbusServerNetwork : IModbusServerNetwork
    {
        private readonly Dictionary<byte, IModbusServer> servers = new Dictionary<byte, IModbusServer>();
        private readonly IModbusTransport transport;

        public ModbusServerNetwork(IModbusTransport transport)
        {
            this.transport = transport ?? throw new ArgumentNullException(nameof(transport));
        }

        public void AddServer(IModbusServer server)
        {
            servers.Add(server.UnitNumber, server);
        }

        public void RemoveServer(IModbusServer server)
        {
            servers.Remove(server.UnitNumber);
        }

        public async Task ListenAsync(CancellationToken cancellationToken)
        {
            while(!cancellationToken.IsCancellationRequested)
            {
                var applicationDataUnit = await transport.ReceiveAsync(cancellationToken);

                if (applicationDataUnit.UnitNumber == 0)
                {
                    foreach(var server in servers.Values)
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
                            await transport.SendAsync(new ApplicationDataUnit(applicationDataUnit.UnitNumber, response), cancellationToken);
                        }
                    }
                }
            }
        }
    }
}
