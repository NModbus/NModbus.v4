using NModbus.Transport.IP.Mbap;
using Shouldly;

namespace NModbus.Transport.IP.Tests
{
    public class MbapSerializerTests
    {
        [Theory]
        [InlineData(42, 3, 1, new byte[] { 0, 42, 0, 0, 0, 3, 1 })]
        public void SerializeMbapHeader_ShouldWork(ushort transactionIdentifier, ushort length, byte unitIdentifier, byte[] expectedBytes)
        {
            MbapSerializer.SerializeMbapHeader(transactionIdentifier, length, unitIdentifier)
               .ShouldBe(expectedBytes);
        }
    }
}
