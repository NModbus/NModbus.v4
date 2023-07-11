using NModbus.Interfaces;
using System.Reactive.Linq;

namespace NModbus.Tests.Transport
{
    public class TestingTransport : IModbusClientTransport
    {
        private readonly IObservable<ModbusMessage> receive;
        private readonly IObserver<ModbusMessage> transmit;

        public TestingTransport(IObservable<ModbusMessage> receive, IObserver<ModbusMessage> transmit)
        {
            this.receive = receive ?? throw new ArgumentNullException(nameof(receive));
            this.transmit = transmit ?? throw new ArgumentNullException(nameof(transmit));
        }

        public Task SendAsync(ModbusMessage applicationDataUnit, CancellationToken cancellationToken = default)
        {
            //Encode the number of bytes to be sent
            transmit.OnNext(applicationDataUnit);

            return Task.CompletedTask;
        }

        public async Task<ModbusMessage> SendAndReceiveAsync(ModbusMessage applicationDataUnit, CancellationToken cancellationToken = default)
        {
            await SendAsync(applicationDataUnit, cancellationToken);

            return await ReceiveAsync();
        }

        public Task<ModbusMessage> ReceiveAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult(receive.Next().First());
        }

        public ValueTask DisposeAsync()
        {
            return ValueTask.CompletedTask;
        }
    }
}
