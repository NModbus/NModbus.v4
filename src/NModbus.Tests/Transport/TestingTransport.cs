using NModbus.Interfaces;
using System.Reactive.Linq;

namespace NModbus.Tests.Transport
{
    public class TestingTransport : IModbusClientTransport
    {
        private readonly IObservable<IModbusMessage> receive;
        private readonly IObserver<IModbusMessage> transmit;

        public TestingTransport(IObservable<IModbusMessage> receive, IObserver<IModbusMessage> transmit)
        {
            this.receive = receive ?? throw new ArgumentNullException(nameof(receive));
            this.transmit = transmit ?? throw new ArgumentNullException(nameof(transmit));
        }

        public Task SendAsync(IModbusMessage message, CancellationToken cancellationToken = default)
        {
            //Encode the number of bytes to be sent
            transmit.OnNext(message);

            return Task.CompletedTask;
        }

        public async Task<IModbusMessage> SendAndReceiveAsync(IModbusMessage message, CancellationToken cancellationToken = default)
        {
            await SendAsync(message, cancellationToken);

            return await ReceiveAsync();
        }

        public Task<IModbusMessage> ReceiveAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult(receive.Next().First());
        }

        public ValueTask DisposeAsync()
        {
            return ValueTask.CompletedTask;
        }
    }
}
