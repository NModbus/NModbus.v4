using NModbus.EndianTools;

namespace NModbus.Messages
{
    public class ReadHoldingRegistersMessageSerializer : ModbusMessageSerializer<ReadHoldingRegistersRequest, ReadHoldingRegistersResponse>
    {
        protected async override Task SerializeRequestCoreAsync(ReadHoldingRegistersRequest request, EndianWriter writer, CancellationToken cancellationToken)
        {
            await writer.WriteAsync(request.StartingAddress, cancellationToken);
            await writer.WriteAsync(request.QuantityOfRegisters, cancellationToken);
        }

        protected override async Task SeserializeResponseCoreAsync(ReadHoldingRegistersResponse response, EndianWriter writer, CancellationToken cancellationToken)
        {
            await writer.WriteAsync(response.ByteCount, cancellationToken);

            foreach (var registerValue in response.RegisterValues)
            {
                await writer.WriteAsync(registerValue, cancellationToken);
            }
        }

        protected async override Task<ReadHoldingRegistersRequest> DeserializeRequestCoreAsync(EndianReader reader, CancellationToken cancellationToken)
        {
            return new ReadHoldingRegistersRequest
            {
                StartingAddress = await reader.ReadUInt16Async(cancellationToken),
                QuantityOfRegisters = await reader.ReadUInt16Async(cancellationToken)
            };
        }

        protected async override Task<ReadHoldingRegistersResponse> DeserializeResponseCoreAsync(EndianReader reader, CancellationToken cancellationToken)
        {
            var byteCount = await reader.ReadByteAsync(cancellationToken);
            var registerCount = byteCount / 2;

            var registerValues = new ushort[registerCount];

            for (var index = 0; index < registerCount; index++)
            {
                registerValues[index] = await reader.ReadUInt16Async(cancellationToken);
            }

            return new ReadHoldingRegistersResponse
            {
                ByteCount = byteCount,
                RegisterValues = registerValues
            };
        }
    }

    public class ReadHoldingRegistersRequest
    {
        public ushort StartingAddress { get; set; }

        public ushort QuantityOfRegisters { get; set; }
    }

    public class ReadHoldingRegistersResponse
    {
        public byte ByteCount { get; set; }

        public ushort[] RegisterValues { get; set; }
    }
}
