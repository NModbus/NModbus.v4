using NModbus.Endian;

namespace NModbus.Messages
{
    public class ReadFileRecordMessageSerializer : ModbusMessageSerializer<ReadFileRecordRequest, ReadFileRecordResponse>
    {
        protected override void SerializeRequestCore(ReadFileRecordRequest request, EndianWriter writer)
        {
            throw new NotImplementedException();
        }

        protected override void SerializeResponseCore(ReadFileRecordResponse response, EndianWriter writer)
        {
            throw new NotImplementedException();
        }

        protected override ReadFileRecordRequest DeserializeRequestCore(EndianReader reader)
        {
            throw new NotImplementedException();
        }

        protected override ReadFileRecordResponse DeserializeResponseCore(EndianReader reader)
        {
            throw new NotImplementedException();
        }
    }

    public record ReadFileRecordRequest();

    public record ReadFileRecordResponse();
    
}
