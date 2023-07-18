using NModbus.Helpers;

namespace NModbus.Tests.Helpers
{
    public class BitPackerShould
    {
        [Fact]
        public void UnpackShouldWork()
        {
            var bytes = new byte[] { 0xcd, 0x6b, 0x05 };

            var unpacked =  BitPacker.Unpack(bytes, 19);

            var expectedBits = "10110011 11010110 101".Replace(" ", "")
                .Select(c => c == '1')
                .ToArray();

            unpacked.ShouldBe(expectedBits);
        }

        [Fact]
        public void PackShouldWork()
        { 
            var bits = "10110011 11010110 101".Replace(" ", "")
                .Select(c => c == '1')
                .ToArray();

            var packed = BitPacker.Pack(bits);

            var expectedBytes = new byte[] { 0xcd, 0x6b, 0x05 };

            packed.ShouldBe(expectedBytes);
        }
    }
}
