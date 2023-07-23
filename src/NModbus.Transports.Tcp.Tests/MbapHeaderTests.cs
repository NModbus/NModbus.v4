using NModbus.Transport.Tcp.TcpMessages;
using Shouldly;

namespace NModbus.Transports.Tcp.Tests
{
    public class MbapHeaderTests
    {
        [Theory]
        [InlineData(42, 3, 1, new byte[] { 0, 42, 0, 0, 0, 3, 1 })]
        public void MbapHeader_ShouldSerialize(ushort transactionIdentifier, ushort length, byte unitIdentifier, byte[] expectedBytes)
        {
            MbapHeaderSerializer.SerializeMbapHeader(transactionIdentifier, length, unitIdentifier)
               .ShouldBe(expectedBytes);
        }
    }
}
