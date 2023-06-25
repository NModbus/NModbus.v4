using NModbus.Interfaces;
using System.Reactive.Linq;

namespace NModbus.Tests.Transport
{
    public class TestingTransport : IModbusClientTransport
    {
        private readonly IObservable<ApplicationDataUnit> receive;
        private readonly IObserver<ApplicationDataUnit> transmit;

        public TestingTransport(IObservable<ApplicationDataUnit> receive, IObserver<ApplicationDataUnit> transmit)
        {
            this.receive = receive ?? throw new ArgumentNullException(nameof(receive));
            this.transmit = transmit ?? throw new ArgumentNullException(nameof(transmit));
        }

        public Task SendAsync(ApplicationDataUnit applicationDataUnit, CancellationToken cancellationToken = default)
        {
            //Encode the number of bytes to be sent
            transmit.OnNext(applicationDataUnit);

            return Task.CompletedTask;
        }

        public async Task<ApplicationDataUnit> SendAndReceiveAsync(ApplicationDataUnit applicationDataUnit, CancellationToken cancellationToken = default)
        {
            await SendAsync(applicationDataUnit, cancellationToken);

            return await ReceiveAsync();
        }

        public Task<ApplicationDataUnit> ReceiveAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult(receive.Next().First());
        }

        public ValueTask DisposeAsync()
        {
            return ValueTask.CompletedTask;
        }
    }
}
