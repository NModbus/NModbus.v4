using Microsoft.Extensions.Logging;
using NModbus.Interfaces;

namespace NModbus
{
    public class ModbusClient : IModbusClient
    {
        private readonly IModbusTransport transport;
        private readonly ILogger logger;

        public ModbusClient(IModbusTransport transport, ILogger<ModbusClient> logger)
        {
            this.transport = transport ?? throw new ArgumentNullException(nameof(transport));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Task MaskWriteRegisterAsync(byte serverAddress, ushort startingAddress, ushort andMask, ushort orMask, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool[]> ReadCoilsAsync(byte serverAddress, ushort startingAddress, ushort quantity, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool[]> ReadDiscreteInputsAsync(byte serverAddress, ushort startingAddress, ushort quantity, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<ushort[]> ReadFifoQueueAsync(byte serverAddress, ushort fifoPointerAddress, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<ReadReferenceResponse[]> ReadFileRecordAsync(byte serverAddress, ReadReferenceRequest[] requests, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<ushort[]> ReadHoldingRegistersAsync(byte serverAddress, ushort startingAddress, ushort quantity, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<ushort[]> ReadWriteMultipleRegistersAsync(byte serverAddress, ushort startingAddress, ushort[] values, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<WriteReferenceResponse[]> WriteFileRecordAsync(byte serverAddress, WriteReferenceRequest[] requests, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task WriteMultipleCoilsAsync(byte serverAddress, ushort startingAddress, bool[] values, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task WriteMultipleRegistersAsync(byte serverAddress, ushort startingAddress, ushort[] values, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task WriteSingleCoilAsync(byte serverAddress, ushort startingAddress, bool value, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task WriteSingleRegisterAsync(byte serverAddress, ushort startingAddress, ushort value, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
