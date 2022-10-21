using NModbus.Endian;

namespace NModbus.Messages
{
    public class MaskWriteRegisterMessageSerializer : ModbusMessageSerializer<MaskWriteRegisterRequest, MaskWriteRegisterResponse>
    {
        protected override void SerializeRequestCore(MaskWriteRegisterRequest request, EndianWriter writer)
        {
            writer.Write(request.ReferenceAddress);
            writer.Write(request.AndMask);
            writer.Write(request.OrMask);
        }

        protected override void SerializeResponseCore(MaskWriteRegisterResponse response, EndianWriter writer)
        {
            writer.Write(response.ReferenceAddress);
            writer.Write(response.AndMask);
            writer.Write(response.OrMask);
        }

        protected override MaskWriteRegisterRequest DeserializeRequestCore(EndianReader reader)
        {
            var referenceAddress = reader.ReadUInt16();
            var andMask = reader.ReadUInt16();
            var orMask = reader.ReadUInt16();

            return new MaskWriteRegisterRequest(referenceAddress, andMask, orMask);
        }

        protected override MaskWriteRegisterResponse DeserializeResponseCore(EndianReader reader)
        {
            var referenceAddress = reader.ReadUInt16();
            var andMask = reader.ReadUInt16();
            var orMask = reader.ReadUInt16();

            return new MaskWriteRegisterResponse(referenceAddress, andMask, orMask);
        }
    }

    public record MaskWriteRegisterRequest(ushort ReferenceAddress, ushort AndMask, ushort OrMask);

    public record MaskWriteRegisterResponse(ushort ReferenceAddress, ushort AndMask, ushort OrMask);
}
