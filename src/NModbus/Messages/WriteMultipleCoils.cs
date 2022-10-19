using NModbus.Endian;

namespace NModbus.Messages
{
    public class WriteMultipleCoilsMessageSerializer : ModbusMessageSerializer<WriteMultipleCoilsRequest, WriteMultipleCoilsResponse>
    {
        protected override void SerializeRequestCore(WriteMultipleCoilsRequest request, EndianWriter writer)
        {
            writer.Write(request.StartingAddress);
            writer.Write((ushort)request.OutputsValue.Length);
            writer.Write((byte)(request.OutputsValue.Length / 8));
            
            //TODO: Do bit math to write the bits
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
