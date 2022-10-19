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

        protected override void SeserializeResponseCore(WriteSingleRegisterResponse response, EndianWriter writer)
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
