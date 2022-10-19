using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NModbus.BasicServer;
using NModbus.Interfaces;
using NModbus.Tests.Transport;
using System.Reactive.Subjects;

namespace NModbus.Tests.Integration
{
    public abstract class ClientServerBase : IDisposable
    {
        private readonly Subject<ApplicationDataUnit> a;
        private readonly Subject<ApplicationDataUnit> b;
        private readonly ILoggerFactory loggerFactory = new NullLoggerFactory();
        private readonly IModbusTransport serverTransport;
        private readonly IModbusTransport clientTransport;
        private readonly IModbusServerNetwork serverNetwork;
        private readonly Task serverListenTask;

        
        protected const byte UnitNumber = 1;

        public ClientServerBase()
        {
            a = new Subject<ApplicationDataUnit>();
            b = new Subject<ApplicationDataUnit>();

            serverTransport = new TestingTransport(a, b);
            clientTransport = new TestingTransport(b, a);

            serverNetwork = new ModbusServerNetwork(serverTransport);

            Storage = new Storage();

            var serverFunctions = ServerFunctionFactory.CreateBasicServerFunctions(loggerFactory, Storage);

            var server = new ModbusServer(UnitNumber, serverFunctions, loggerFactory.CreateLogger<ModbusServer>());

            serverNetwork.AddServer(server);

            serverListenTask = Task.Factory.StartNew(async () => await serverNetwork.ListenAsync(CancellationToken.None), TaskCreationOptions.LongRunning);

            Client = new ModbusClient(clientTransport, loggerFactory.CreateLogger<ModbusClient>());
        }

        protected IModbusClient Client { get; }

        protected Storage Storage { get; }

        public void Dispose()
        {
            //serverListenTask.Dispose();
            a.Dispose();
            b.Dispose();
            serverTransport.Dispose();
            clientTransport.Dispose();
        }
    }
}
