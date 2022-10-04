using NModbus.EndianTools;

namespace NModbus.Functions
{
    public class WriteSingleRegisterMessageFactory
        : ModbusMessageFactory<WriteSingleRegisterRequest, WriteSingleRegisterResponse>
    {
        public override async Task CreateRequestCoreAsync(WriteSingleRegisterRequest request, EndianWriter writer, CancellationToken cancellationToken)
        {
            await writer.WriteAsync(request.Address);
            await writer.WriteAsync(request.Value);
        }

        public override async Task CreateResponseCoreAsync(WriteSingleRegisterResponse response, EndianWriter writer, CancellationToken cancellationToken)
        {
            await writer.WriteAsync(response.Address);
            await writer.WriteAsync(response.Value);
        }

        public override async Task<WriteSingleRegisterRequest> GetRequestFromDataCoreAsync(EndianReader reader, CancellationToken cancellationToken)
        {
            var address = await reader.ReadUInt16Async(cancellationToken);
            var value = await reader.ReadUInt16Async(cancellationToken);

            return new WriteSingleRegisterRequest(address, value);
        }

        public override async Task<WriteSingleRegisterResponse> GetResponseFromDataCoreAsync(EndianReader reader, CancellationToken cancellationToken)
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
