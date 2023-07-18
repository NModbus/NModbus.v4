using NModbus.Helpers;

namespace NModbus.Tests.Helpers
{
    public class BitPackerShould
    {
        [Fact]
        public void Unpack_ShouldWork()
        {
            var bytes = new byte[] { 0xcd, 0x6b, 0x05 };

            var unpacked =  BitPacker.Unpack(bytes, 19);

            var expectedBits = "10110011 11010110 101".Replace(" ", "")
                .Select(c => c == '1')
                .ToArray();

            unpacked.ShouldBe(expectedBits);
        }

        [Fact]
        public void Pack_ShouldWork()
        { 
            var bits = "10110011 11010110 101".Replace(" ", "")
                .Select(c => c == '1')
                .ToArray();

            var packed = BitPacker.Pack(bits);

            var expectedBytes = new byte[] { 0xcd, 0x6b, 0x05 };

            packed.ShouldBe(expectedBytes);
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(2, 1)]
        [InlineData(7, 1)]
        [InlineData(8, 1)]
        [InlineData(9, 2)]
        [InlineData(16, 2)]
        [InlineData(17, 3)]
        public void CalculateBytesToPackBits(int numberOfBits, int expectedNumberOfBytes)
        {
            BitPacker.CalculateBytesToPackBits(numberOfBits)
                .ShouldBe(expectedNumberOfBytes);
        }

    }
}
