using Divergic.Logging.Xunit;
using Microsoft.Extensions.Logging;
using NModbus.Tests.Transport;
using Xunit.Abstractions;

namespace NModbus.Tests.Integration
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
            var clientServer = new ClientServer(1, loggerFactory);

            //Give the server (TcpListener) time to start up
            await Task.Delay(TimeSpan.FromSeconds(0.1));

            return clientServer;
        }

    }
}
