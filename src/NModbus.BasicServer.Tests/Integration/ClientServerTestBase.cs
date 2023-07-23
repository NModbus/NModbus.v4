using Divergic.Logging.Xunit;
using Microsoft.Extensions.Logging;
using NModbus.BasicServer.Tests.Transport;
using Xunit.Abstractions;

namespace NModbus.BasicServer.Tests.Integration
{
    public abstract class ClientServerTestBase
    {
        protected readonly ILoggerFactory loggerFactory;

        protected ClientServerTestBase(ITestOutputHelper output)
        {
            loggerFactory = LogFactory.Create(output);
        }

        protected async Task<ClientServer> CreateClientServerAsync(byte unitIdentifier)
        {
            var clientServer = new ClientServer(unitIdentifier, loggerFactory);

            //Give the server (TcpListener) time to start up
            await Task.Delay(TimeSpan.FromSeconds(0.1));

            return clientServer;
        }

    }
}
