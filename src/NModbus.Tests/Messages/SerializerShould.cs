using NModbus.Messages;
using Shouldly;

namespace NModbus.Tests.Messages
{
    public class SerializerShould
    {
        [Fact]
        public void SerializeWriteSingleRegisterRequestShould()
        {
            var request = new WriteSingleRegisterRequest(1, 3);

            var serializer = new WriteSingleRegisterMessageSerializer();

            var serialized = serializer.SerializeRequest(request);

            serialized.ShouldBe(new byte[] { 0x00, 0x01, 0x00, 0x03 });
        }

        [Fact]
        public void SerializeWriteSingleRegisterResponseShould()
        {
            var response = new WriteSingleRegisterResponse(1, 3);

            var serializer = new WriteSingleRegisterMessageSerializer();

            var serialized = serializer.SerializeResponse(response);

            serialized.ShouldBe(new byte[] { 0x00, 0x01, 0x00, 0x03 });
        }

        [Fact]
        public void SerializeReadCoilsRequestShould()
        {
            var request = new ReadCoilsRequest(19, 19);

            var serializer = new ReadCoilsMessageSerializer();

            var serialized = serializer.SerializeRequest(request);

            serialized.ShouldBe(new byte[] { 0x00, 0x13, 0x00, 0x13 });
        }

        [Fact]
        public void SerializeReadCoilsResponseShould()
        {
            var response = new ReadCoilsResponse(new byte[] { 0xcd, 0x6b, 0x05 });

            var serializer = new ReadCoilsMessageSerializer();

            var serialized = serializer.SerializeResponse(response);

            serialized.ShouldBe(new byte[] { 0x03, 0xcd, 0x6b, 0x05 });
        }
    }
}
