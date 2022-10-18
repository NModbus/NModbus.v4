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
            byte byteCount = (byte)(response.RegisterValues.Length * 2);

            writer.Write(byteCount);

            foreach (var registerValue in response.RegisterValues)
            {
                writer.Write(registerValue);
            }
        }

        protected override ReadHoldingRegistersRequest DeserializeRequestCore(EndianReader reader)
        {
            var startingAddress = reader.ReadUInt16();
            var quantityOfRegisters = reader.ReadUInt16();

            return new ReadHoldingRegistersRequest(startingAddress, quantityOfRegisters);
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

            return new ReadHoldingRegistersResponse(registerValues);
        }
    }

    public class ReadHoldingRegistersRequest
    {
        public ReadHoldingRegistersRequest(ushort startingAddress, ushort quantityOfRegisters)
        {
            StartingAddress = startingAddress;
            QuantityOfRegisters = quantityOfRegisters;
        }

        public ushort StartingAddress { get; }

        public ushort QuantityOfRegisters { get; }
    }

    public class ReadHoldingRegistersResponse
    {
        public ReadHoldingRegistersResponse(ushort[] registerValues)
        {
            RegisterValues = registerValues ?? throw new ArgumentNullException(nameof(registerValues));
        }

        public ushort[] RegisterValues { get; }
    }
}
