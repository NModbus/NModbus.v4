using NModbus.Endian;
using NModbus.Helpers;

namespace NModbus.Messages
{
    public class WriteMultipleCoilsMessageSerializer : ModbusMessageSerializer<WriteMultipleCoilsRequest, WriteMultipleCoilsResponse>
    {
        protected override void SerializeRequestCore(WriteMultipleCoilsRequest request, EndianWriter writer)
        {
            var packed = BitPacker.Pack(request.OutputsValue);

            writer.Write(request.StartingAddress);
            writer.Write((ushort)request.OutputsValue.Length);
            writer.Write((byte)(packed.Length));
            writer.Write(packed);
        }

        protected override void SerializeResponseCore(WriteMultipleCoilsResponse response, EndianWriter writer)
        {
            throw new NotImplementedException();
        }

        protected override WriteMultipleCoilsRequest DeserializeRequestCore(EndianReader reader)
        {
            throw new NotImplementedException();
        }

        protected override WriteMultipleCoilsResponse DeserializeResponseCore(EndianReader reader)
        {
            throw new NotImplementedException();
        }
    }

    public record WriteMultipleCoilsRequest(ushort StartingAddress, bool[] OutputsValue);
    

    public record WriteMultipleCoilsResponse(ushort StartingAddress, ushort QuantityOfOutputs);
    
}
