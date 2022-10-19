using NModbus.Endian;

namespace NModbus.Messages
{
    public class WriteSingleRegisterMessageSerializer
        : ModbusMessageSerializer<WriteSingleRegisterRequest, WriteSingleRegisterResponse>
    {
        protected override void SerializeRequestCore(WriteSingleRegisterRequest request, EndianWriter writer)
        {
            writer.Write(request.Address);
            writer.Write(request.Value);
        }

        protected override void SerializeResponseCore(WriteSingleRegisterResponse response, EndianWriter writer)
        {
            writer.Write(response.Address);
            writer.Write(response.Value);
        }

        protected override WriteSingleRegisterRequest DeserializeRequestCore(EndianReader reader)
        {
            var address = reader.ReadUInt16();
            var value = reader.ReadUInt16();

            return new WriteSingleRegisterRequest(address, value);
        }

        protected override WriteSingleRegisterResponse DeserializeResponseCore(EndianReader reader)
        {
            var address = reader.ReadUInt16();
            var value = reader.ReadUInt16();

            return new WriteSingleRegisterResponse(address, value);
        }
    }

    public record WriteSingleRegisterRequest(ushort Address, ushort Value);
    
    public record WriteSingleRegisterResponse(ushort Address, ushort Value);
}
