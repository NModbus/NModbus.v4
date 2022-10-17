using NModbus.Messages;
using Shouldly;

namespace NModbus.Tests.Messages
{
    public class SerializerShould
    {
        [Fact]
        public async Task SerializeWriteSingleRegisterRequestShould()
        {
            var request = new WriteSingleRegisterRequest(1, 3);

            var factory = new WriteSingleRegisterMessageSerializer();

            var data = await factory.SerializeRequestAsync(request);

            data.ShouldBe(new byte[] { 0x00, 0x01, 0x00, 0x03 });
        }

        [Fact]
        public async Task SerializeWriteSingleRegisterResponseShould()
        {
            var response = new WriteSingleRegisterResponse(1, 3);

            var factory = new WriteSingleRegisterMessageSerializer();

            var data = await factory.SerializeResponseAsync(response);

            data.ShouldBe(new byte[] { 0x00, 0x01, 0x00, 0x03 });
        }
    }
}
