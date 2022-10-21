using NModbus.Endian;

namespace NModbus.Messages
{
    public class WriteFileRecordMessageSerializer : ModbusMessageSerializer<WriteFileRecordRequest, WriteFileRecordResponse>
    {
        protected override void SerializeRequestCore(WriteFileRecordRequest request, EndianWriter writer)
        {
            throw new NotImplementedException();
        }

        protected override void SerializeResponseCore(WriteFileRecordResponse response, EndianWriter writer)
        {
            throw new NotImplementedException();
        }

        protected override WriteFileRecordRequest DeserializeRequestCore(EndianReader reader)
        {
            throw new NotImplementedException();
        }

        protected override WriteFileRecordResponse DeserializeResponseCore(EndianReader reader)
        {
            throw new NotImplementedException();
        }
    }

    public record WriteFileRecordRequest();

    public record WriteFileRecordResponse();
}
