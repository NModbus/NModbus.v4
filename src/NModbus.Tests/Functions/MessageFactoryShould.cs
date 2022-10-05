using NModbus.Functions;
using Shouldly;

namespace NModbus.Tests.Functions
{
    public class MessageFactoryShould
    {
        [Fact]
        public async Task WriteSingleRegisterRequestShould()
        {
            var request = new WriteSingleRegisterRequest(1, 3);

            var factory = new WriteSingleRegisterMessageFactory();

            var data = await factory.SerializeRequestAsync(request);

            data.ShouldBe(new byte[] { 0x00, 0x01, 0x00, 0x03 });
        }

        [Fact]
        public async Task WriteSingleRegisterResponseShould()
        {
            var response = new WriteSingleRegisterResponse(1, 3);

            var factory = new WriteSingleRegisterMessageFactory();

            var data = await factory.SerializeResponseAsync(response);

            data.ShouldBe(new byte[] { 0x00, 0x01, 0x00, 0x03 });
        }
    }
}
