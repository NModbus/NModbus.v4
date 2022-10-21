using NModbus.Endian;

namespace NModbus.Messages
{
    public class ReadFifoQueueMessageSerializer : ModbusMessageSerializer<ReadFifoQueueRequest, ReadFifoQueueResponse>
    {
        protected override void SerializeRequestCore(ReadFifoQueueRequest request, EndianWriter writer)
        {
            writer.Write(request.FifoPointerAddress);
        }

        protected override void SerializeResponseCore(ReadFifoQueueResponse response, EndianWriter writer)
        {
            writer.Write((ushort)(response.FifoValueRegister.Length * 2));
            writer.Write((ushort)response.FifoValueRegister.Length);
            writer.Write(response.FifoValueRegister);
        }

        protected override ReadFifoQueueRequest DeserializeRequestCore(EndianReader reader)
        {
            var fifoPointerAddress = reader.ReadUInt16();

            return new ReadFifoQueueRequest(fifoPointerAddress);
        }

        protected override ReadFifoQueueResponse DeserializeResponseCore(EndianReader reader)
        {
            var byteCount = reader.ReadUInt16();
            var fifoCount = reader.ReadUInt16();
            var fifoValueRegister = reader.ReadUInt16Array(fifoCount);

            return new ReadFifoQueueResponse(fifoValueRegister);
        }
    }

    public record ReadFifoQueueRequest(ushort FifoPointerAddress);

    public record ReadFifoQueueResponse(ushort[] FifoValueRegister);
}
