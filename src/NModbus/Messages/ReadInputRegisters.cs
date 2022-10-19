using NModbus.Endian;

namespace NModbus.Messages
{
    public class ReadInputRegistersMessageSerializer : ModbusMessageSerializer<ReadInputRegistersRequest, ReadInputRegistersResponse>
    {
        protected override void SerializeRequestCore(ReadInputRegistersRequest request, EndianWriter writer)
        {
            writer.Write(request.StartingAddress);
            writer.Write(request.QuantityOfInputRegisters);
        }

        protected override void SerializeResponseCore(ReadInputRegistersResponse response, EndianWriter writer)
        {
            writer.Write((byte)(response.InputRegisters.Length * 2));
            writer.Write(response.InputRegisters);
        }

        protected override ReadInputRegistersRequest DeserializeRequestCore(EndianReader reader)
        {
            var startingAddress = reader.ReadUInt16();
            var quantityOfInputRegisters = reader.ReadUInt16();

            return new ReadInputRegistersRequest(startingAddress, quantityOfInputRegisters);
        }

        protected override ReadInputRegistersResponse DeserializeResponseCore(EndianReader reader)
        {
            var byteCount = reader.ReadByte();
            var inputRegisters = reader.ReadUInt16Array(byteCount / 2);

            return new ReadInputRegistersResponse(inputRegisters);
        }
    }

    public record ReadInputRegistersRequest(ushort StartingAddress, ushort QuantityOfInputRegisters);

    public record ReadInputRegistersResponse(ushort[] InputRegisters);
}
