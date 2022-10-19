using NModbus.Endian;

namespace NModbus.Messages
{
    public class ReadWriteMultipleRegistersSerializer : ModbusMessageSerializer<ReadWriteMultipeRegistersRequest, ReadWriteMultipeRegistersResponse>
    {
        protected override void SerializeRequestCore(ReadWriteMultipeRegistersRequest request, EndianWriter writer)
        {
            writer.Write(request.ReadStartingAddress);
            writer.Write(request.QuantityToRead);
            writer.Write(request.WriteStartingAddress);
            writer.Write((ushort)request.WriteRegistersValue.Length);
            writer.Write((byte)(request.WriteRegistersValue.Length * 2));
            writer.Write(request.WriteRegistersValue);
        }

        protected override void SerializeResponseCore(ReadWriteMultipeRegistersResponse response, EndianWriter writer)
        {
            writer.Write((byte)(response.ReadRegistersValue.Length * 2));
            writer.Write(response.ReadRegistersValue);
        }

        protected override ReadWriteMultipeRegistersRequest DeserializeRequestCore(EndianReader reader)
        {
            var readStartingAddress = reader.ReadByte();
            var quantityToRead = reader.ReadUInt16();
            var writeStartingAddress = reader.ReadUInt16();
            var quantityToWrite = reader.ReadUInt16();
            var writeByteCount = reader.ReadByte();
            var writeRegistersValue = reader.ReadUInt16Array(writeByteCount / 2);

            return new ReadWriteMultipeRegistersRequest(readStartingAddress, quantityToRead, writeStartingAddress, writeRegistersValue);
        }

        protected override ReadWriteMultipeRegistersResponse DeserializeResponseCore(EndianReader reader)
        {
            var byteCount = reader.ReadByte();
            var readRegistersValue = reader.ReadUInt16Array(byteCount / 2);

            return new ReadWriteMultipeRegistersResponse(readRegistersValue);
        }
    }

    public record ReadWriteMultipeRegistersRequest(
        ushort ReadStartingAddress, 
        ushort QuantityToRead, 
        ushort WriteStartingAddress, 
        ushort[] WriteRegistersValue);

    public record ReadWriteMultipeRegistersResponse(ushort[] ReadRegistersValue);
}
