using NModbus.EndianTools;

namespace NModbus.Messages
{
    public class ReadHoldingRegistersMessageSerializer : ModbusMessageSerializer<ReadHoldingRegistersRequest, ReadHoldingRegistersResponse>
    {
        protected override void SerializeRequestCore(ReadHoldingRegistersRequest request, EndianWriter writer)
        {
            writer.Write(request.StartingAddress);
            writer.Write(request.QuantityOfRegisters);
        }

        protected override void SeserializeResponseCore(ReadHoldingRegistersResponse response, EndianWriter writer)
        {
            writer.Write(response.ByteCount);

            foreach (var registerValue in response.RegisterValues)
            {
                writer.Write(registerValue);
            }
        }

        protected override ReadHoldingRegistersRequest DeserializeRequestCore(EndianReader reader)
        {
            return new ReadHoldingRegistersRequest
            {
                StartingAddress = reader.ReadUInt16(),
                QuantityOfRegisters = reader.ReadUInt16()
            };
        }

        protected override ReadHoldingRegistersResponse DeserializeResponseCore(EndianReader reader)
        {
            var byteCount = reader.ReadByte();
            var registerCount = byteCount / 2;

            var registerValues = new ushort[registerCount];

            for (var index = 0; index < registerCount; index++)
            {
                registerValues[index] = reader.ReadUInt16();
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
