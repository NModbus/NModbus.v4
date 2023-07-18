using Divergic.Logging.Xunit;
using Microsoft.Extensions.Logging;
using Moq;
using NModbus.BasicServer.Functions;
using NModbus.BasicServer.Interfaces;
using NModbus.Messages;
using Shouldly;
using Xunit.Abstractions;

namespace NModbus.BasicServer.Tests
{
    public class Functions
    {

        private readonly ILoggerFactory loggerFactory;

        public Functions(ITestOutputHelper output)
        {
            loggerFactory = LogFactory.Create(output);
        }

        [Theory]
        [InlineData(24, new ushort[] { 4, 5, 6 })]
        [InlineData(22, new ushort[] { 78 })]
        public async Task WriteHoldingRegisters_ShouldWork(ushort startingAddress, ushort[] values)
        {
            var storageMock = new Mock<IDevicePointStorage<ushort>>();

            var implementation = new WriteMultipleRegistersImplementation(loggerFactory, storageMock.Object);

            var request = new WriteMultipleRegistersRequest(startingAddress, values);

            await implementation.ProcessAsync(request, default);

            storageMock.Verify(m => m.WritePoints(startingAddress, values), Times.Once);
        }

        [Theory]
        [InlineData(50, new bool[] { true, true, false, false, false, false, false, true })]
        public async Task ReadCoils_ShouldWork(ushort startingAddress, bool[] values)
        {
            var storage = new SparsePointStorage<bool>();

            for(int index = 0; index < values.Length; index++)
            {
                storage[(ushort)(index + startingAddress)] = values[index];
            }

            var implementation = new ReadCoilsImplementation(loggerFactory, storage);

            var request = new ReadCoilsRequest(startingAddress, (ushort)values.Length);

            var response = await implementation.ProcessAsync(request, default);

            response.Unpack((ushort)values.Length).ShouldBe(values);
        }
    }
}

