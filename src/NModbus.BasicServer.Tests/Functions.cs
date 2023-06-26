using Divergic.Logging.Xunit;
using Microsoft.Extensions.Logging;
using Moq;
using NModbus.BasicServer.Functions;
using NModbus.BasicServer.Interfaces;
using NModbus.Messages;
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
    }
}

