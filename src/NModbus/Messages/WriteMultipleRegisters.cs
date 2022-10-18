using NModbus.EndianTools;

namespace NModbus.Messages
{
    public class WriteMultipleRegistersMessageSerializer : ModbusMessageSerializer<WriteMultipleRegistersRequest, WriteMultipleRegistersResponse>
    {
        protected  override async Task SerializeRequestCoreAsync(WriteMultipleRegistersRequest request, EndianWriter writer, CancellationToken cancellationToken)
        {
            await writer.WriteAsync(request.StartingAddress, cancellationToken);
            await writer.WriteAsync((ushort)(request.Registers.Length), cancellationToken);
            await writer.WriteAsync((byte)(request.Registers.Length * 2), cancellationToken);
            await writer.WriteAsync(request.Registers);
        }

        protected override async Task SeserializeResponseCoreAsync(WriteMultipleRegistersResponse response, EndianWriter writer, CancellationToken cancellationToken)
        {
            await writer.WriteAsync(response.StartingAddress);
            await writer.WriteAsync(response.QuantityOfRegisters);
        }

        protected override async Task<WriteMultipleRegistersRequest> DeserializeRequestCoreAsync(EndianReader reader, CancellationToken cancellationToken)
        {
            var startingAddress = await reader.ReadUInt16Async(cancellationToken);
            var quantityOfRegisters = await reader.ReadUInt16Async(cancellationToken);
            var byteCount = await reader.ReadByteAsync(cancellationToken);

            //TODO: Reconcile quantityOfRegisters with byteCount

            var registers = await reader.ReadUInt16ArrayAsync(quantityOfRegisters, cancellationToken);

            return new WriteMultipleRegistersRequest(startingAddress, registers);
        }

        protected override async Task<WriteMultipleRegistersResponse> DeserializeResponseCoreAsync(EndianReader reader, CancellationToken cancellationToken)
        {
            var startingAddress = await reader.ReadUInt16Async(cancellationToken);
            var quantityOfRegisters = await reader.ReadUInt16Async(cancellationToken);

            return new WriteMultipleRegistersResponse(startingAddress, quantityOfRegisters);
        }
    }

    public class WriteMultipleRegistersRequest
    {
        public WriteMultipleRegistersRequest(ushort startingAddress, ushort[] registers)
        {
            StartingAddress = startingAddress;
            Registers = registers ?? throw new ArgumentNullException(nameof(registers));
        }

        public ushort StartingAddress { get; }

        public ushort[] Registers { get; }
    }

    public class WriteMultipleRegistersResponse
    {
        public WriteMultipleRegistersResponse(ushort startingAddress, ushort quantityOfRegisters)
        {
            StartingAddress = startingAddress;
            QuantityOfRegisters = quantityOfRegisters;
        }

        public ushort StartingAddress { get; }

        public ushort QuantityOfRegisters { get; }
    }
}
