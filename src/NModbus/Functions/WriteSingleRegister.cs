using NModbus.EndianTools;

namespace NModbus.Functions
{
    public class WriteSingleRegisterMessageSerializer
        : ModbusMessageSerializer<WriteSingleRegisterRequest, WriteSingleRegisterResponse>
    {
        protected override async Task SerializeRequestCoreAsync(WriteSingleRegisterRequest request, EndianWriter writer, CancellationToken cancellationToken)
        {
            await writer.WriteAsync(request.Address);
            await writer.WriteAsync(request.Value);
        }

        protected override async Task SeserializeResponseCoreAsync(WriteSingleRegisterResponse response, EndianWriter writer, CancellationToken cancellationToken)
        {
            await writer.WriteAsync(response.Address);
            await writer.WriteAsync(response.Value);
        }

        protected override async Task<WriteSingleRegisterRequest> DeserializeRequestCoreAsync(EndianReader reader, CancellationToken cancellationToken)
        {
            var address = await reader.ReadUInt16Async(cancellationToken);
            var value = await reader.ReadUInt16Async(cancellationToken);

            return new WriteSingleRegisterRequest(address, value);
        }

        protected override async Task<WriteSingleRegisterResponse> DeserializeResponseCoreAsync(EndianReader reader, CancellationToken cancellationToken)
        {
            var address = await reader.ReadUInt16Async(cancellationToken);
            var value = await reader.ReadUInt16Async(cancellationToken);

            return new WriteSingleRegisterResponse(address, value);
        }
    }

    public class WriteSingleRegisterRequest
    {
        public WriteSingleRegisterRequest(ushort address, ushort value)
        {
            Address = address;
            Value = value;
        }

        public ushort Address { get; }

        public ushort Value { get; }
    }

    public class WriteSingleRegisterResponse
    {
        public WriteSingleRegisterResponse(ushort address, ushort value)
        {
            Address = address;
            Value = value;
        }

        public ushort Address { get; }

        public ushort Value { get; }
    }
}
