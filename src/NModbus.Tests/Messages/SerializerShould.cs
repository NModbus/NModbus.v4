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

            var factory = new WriteSingleRegisterMessageSerializer();

            var data = factory.SerializeRequest(request);

            data.ShouldBe(new byte[] { 0x00, 0x01, 0x00, 0x03 });
        }

        [Fact]
        public void SerializeWriteSingleRegisterResponseShould()
        {
            var response = new WriteSingleRegisterResponse(1, 3);

            var factory = new WriteSingleRegisterMessageSerializer();

            var data = factory.SerializeResponse(response);

            data.ShouldBe(new byte[] { 0x00, 0x01, 0x00, 0x03 });
        }
    }
}
