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

    public struct ReadReferenceRequest
    {
        public ReferenceType ReferenceType { get; }

        /// <summary>
        /// While it is allowed for the File Number to be in the range 1 to 0xFFFF, it should be noted that 
        /// interoperability with legacy equipment may be compromised if the File Number is greater than 
        /// 10 (0x0A).
        /// </summary>
        public ushort FileNumber { get; }

        /// <summary>
        /// 0x0000 to 0x270F
        /// </summary>
        public ushort RecordNumber { get; }

        /// <summary>
        /// The amount of data to read.
        /// </summary>
        public ushort RecordLength { get; }
    }

}
