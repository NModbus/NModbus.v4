using NModbus.Endian;

namespace NModbus.Messages
{
    public class ReadHoldingRegistersMessageSerializer : ModbusMessageSerializer<ReadHoldingRegistersRequest, ReadHoldingRegistersResponse>
    {
        protected override void SerializeRequestCore(ReadHoldingRegistersRequest request, EndianWriter writer)
        {
            writer.Write(request.StartingAddress);
            writer.Write(request.QuantityOfRegisters);
        }

        protected override void SerializeResponseCore(ReadHoldingRegistersResponse response, EndianWriter writer)
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

    public record ReadHoldingRegistersRequest(ushort StartingAddress, ushort QuantityOfRegisters);

    public record ReadHoldingRegistersResponse(ushort[] RegisterValues);
}
