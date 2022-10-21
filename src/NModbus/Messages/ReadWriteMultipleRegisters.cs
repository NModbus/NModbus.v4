using NModbus.Endian;

namespace NModbus.Messages
{
    public class ReadWriteMultipleRegistersMessageSerializer : ModbusMessageSerializer<ReadWriteMultipleRegistersRequest, ReadWriteMultipleRegistersResponse>
    {
        protected override void SerializeRequestCore(ReadWriteMultipleRegistersRequest request, EndianWriter writer)
        {
            writer.Write(request.ReadStartingAddress);
            writer.Write(request.QuantityToRead);
            writer.Write(request.WriteStartingAddress);
            writer.Write((ushort)request.WriteRegistersValue.Length);
            writer.Write((byte)(request.WriteRegistersValue.Length * 2));
            writer.Write(request.WriteRegistersValue);
        }

        protected override void SerializeResponseCore(ReadWriteMultipleRegistersResponse response, EndianWriter writer)
        {
            writer.Write((byte)(response.ReadRegistersValue.Length * 2));
            writer.Write(response.ReadRegistersValue);
        }

        protected override ReadWriteMultipleRegistersRequest DeserializeRequestCore(EndianReader reader)
        {
            var readStartingAddress = reader.ReadByte();
            var quantityToRead = reader.ReadUInt16();
            var writeStartingAddress = reader.ReadUInt16();
            var quantityToWrite = reader.ReadUInt16();
            var writeByteCount = reader.ReadByte();
            var writeRegistersValue = reader.ReadUInt16Array(writeByteCount / 2);

            return new ReadWriteMultipleRegistersRequest(readStartingAddress, quantityToRead, writeStartingAddress, writeRegistersValue);
        }

        protected override ReadWriteMultipleRegistersResponse DeserializeResponseCore(EndianReader reader)
        {
            var byteCount = reader.ReadByte();
            var readRegistersValue = reader.ReadUInt16Array(byteCount / 2);

            return new ReadWriteMultipleRegistersResponse(readRegistersValue);
        }
    }

    public record ReadWriteMultipleRegistersRequest(
        ushort ReadStartingAddress, 
        ushort QuantityToRead, 
        ushort WriteStartingAddress, 
        ushort[] WriteRegistersValue);

    public record ReadWriteMultipleRegistersResponse(ushort[] ReadRegistersValue);
}
