using NModbus.Endian;

namespace NModbus.Messages
{
    public class ReadCoilsMessageSerializer : ModbusMessageSerializer<ReadCoilsRequest, ReadCoilsResponse>
    {
        protected override void SerializeRequestCore(ReadCoilsRequest request, EndianWriter writer)
        {
            writer.Write(request.StartingAddress);
            writer.Write(request.QuantityOfOutputs);
        }

        protected override void SerializeResponseCore(ReadCoilsResponse response, EndianWriter writer)
        {
            writer.Write((byte)response.CoilStatus.Length);
            writer.Write(response.CoilStatus);
        }

        protected override ReadCoilsRequest DeserializeRequestCore(EndianReader reader)
        {
            var startingAddress = reader.ReadUInt16();
            var quantityOfCoils = reader.ReadUInt16();

            return new ReadCoilsRequest(startingAddress, quantityOfCoils);
        }

        protected override ReadCoilsResponse DeserializeResponseCore(EndianReader reader)
        {
            var byteCount = reader.ReadByte();
            var coilStatus = reader.ReadBytes(byteCount);

            return new ReadCoilsResponse(coilStatus);
        }
    }

    public record ReadCoilsRequest(ushort StartingAddress, ushort QuantityOfOutputs);

    public record ReadCoilsResponse(byte[] CoilStatus);
}
