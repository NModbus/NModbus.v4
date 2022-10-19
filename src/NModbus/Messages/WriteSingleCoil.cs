using NModbus.Endian;

namespace NModbus.Messages
{
    public class WriteSingleCoilMessageSerializer : ModbusMessageSerializer<WriteSingleCoilRequest, WriteSingleCoilResponse>
    {
        private const ushort Off = 0x0000;
        private const ushort On = 0xFF00;

        protected override void SerializeRequestCore(WriteSingleCoilRequest request, EndianWriter writer)
        {
            writer.Write(request.OutputAddress);
            writer.Write(request.OutputValue ? On : Off);
        }

        protected override void SerializeResponseCore(WriteSingleCoilResponse response, EndianWriter writer)
        {
            writer.Write(response.OutputAddress);
            writer.Write(response.OutputValue ? On : Off);
        }

        protected override WriteSingleCoilRequest DeserializeRequestCore(EndianReader reader)
        {
            var outputAddress = reader.ReadUInt16();
            var outputValue = reader.ReadUInt16() == On;

            return new WriteSingleCoilRequest(outputAddress, outputValue);
        }

        protected override WriteSingleCoilResponse DeserializeResponseCore(EndianReader reader)
        {
            var outputAddress = reader.ReadUInt16();
            var outputValue = reader.ReadUInt16() == On;

            return new WriteSingleCoilResponse(outputAddress, outputValue);
        }
    }

    public record WriteSingleCoilRequest(ushort OutputAddress, bool OutputValue);

    public record WriteSingleCoilResponse(ushort OutputAddress, bool OutputValue);
}
