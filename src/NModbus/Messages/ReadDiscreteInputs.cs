using NModbus.Endian;
using NModbus.Helpers;

namespace NModbus.Messages
{
    public class ReadDiscreteInputsMessageSerilizer : ModbusMessageSerializer<ReadDiscreteInputsRequest, ReadDiscreteInputsResponse>
    {
        protected override void SerializeRequestCore(ReadDiscreteInputsRequest request, EndianWriter writer)
        {
            writer.Write(request.StartingAddress);
            writer.Write(request.QuantityOfInputs);
        }

        protected override void SerializeResponseCore(ReadDiscreteInputsResponse response, EndianWriter writer)
        {
            writer.Write((byte)response.InputStatus.Length);
            writer.Write(response.InputStatus);
        }

        protected override ReadDiscreteInputsRequest DeserializeRequestCore(EndianReader reader)
        {
            var startingAddress = reader.ReadUInt16();
            var quantityOfInputs = reader.ReadUInt16();

            return new ReadDiscreteInputsRequest(startingAddress, quantityOfInputs);
        }

        protected override ReadDiscreteInputsResponse DeserializeResponseCore(EndianReader reader)
        {
            var byteCount = reader.ReadByte();
            var inputStatus = reader.ReadBytes(byteCount);

            return new ReadDiscreteInputsResponse(inputStatus);
        }
    }

    public record ReadDiscreteInputsRequest(ushort StartingAddress, ushort QuantityOfInputs);

    public record ReadDiscreteInputsResponse(byte[] InputStatus)
    {
        public bool[] Unpack(ushort quantityOfInputs)
        {
            return BitPacker.Unpack(InputStatus, quantityOfInputs);
        }
    }
}
