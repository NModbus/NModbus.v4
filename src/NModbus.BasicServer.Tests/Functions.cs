using Divergic.Logging.Xunit;
using Microsoft.Extensions.Logging;
using Moq;
using NModbus.BasicServer;
using NModbus.BasicServer.Functions;
using NModbus.BasicServer.Interfaces;
using NModbus.Messages;
using Xunit.Abstractions;

namespace NModbus.Tests
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

        [Theory]
        [InlineData(50, true)]
        public async Task WriteSingleCoil_ShouldWork(ushort outputAddress, bool outputValue)
        {
            var storageMock = new Mock<IDevicePointStorage<bool>>();

            var implementation = new WriteSingleCoilImplementation(loggerFactory, storageMock.Object);

            var request = new WriteSingleCoilRequest(outputAddress, outputValue);

            await implementation.ProcessAsync(request, default);

            storageMock.Verify(m => m.WritePoints(outputAddress, new bool[] { outputValue }), Times.Once);
        }

        [Theory]
        [InlineData(100, new bool[] { true, false, false, false } )]
        public async Task WriteMultipleCoils_ShouldWork(ushort startingAddress, bool[] values)
        {
            var storageMock = new Mock<IDevicePointStorage<bool>>();

            var implementation = new WriteMultipleCoilsImplementation(loggerFactory, storageMock.Object);

            var request = new WriteMultipleCoilsRequest(startingAddress, values);

            await implementation.ProcessAsync(request, default);

            storageMock.Verify(m => m.WritePoints(startingAddress, values), Times.Once);
        }

        [Theory]
        [InlineData(1000, new ushort[] { 1, 2, 3, 4, 5 }, 2000, new ushort[] { 500, 501, 503 })]
        public async Task ReadWriteMultipleRegisters_NonOverlappingShouldWork(ushort ReadStartingAddress, ushort[] ReadValues, ushort writeStartingAddress, ushort[] writeValues)
        {
            var storageMock = new Mock<IDevicePointStorage<ushort>>();

            storageMock.Setup(s => s.ReadPoints(ReadStartingAddress, (ushort)ReadValues.Length))
                .Returns(ReadValues);

            var implementation = new ReadWriteMultipleRegistersImplementation(loggerFactory, storageMock.Object);

            var request = new ReadWriteMultipleRegistersRequest(ReadStartingAddress, (ushort)ReadValues.Length, writeStartingAddress, writeValues);

            var response = await implementation.ProcessAsync(request, default);

            storageMock.Verify(m => m.WritePoints(writeStartingAddress, writeValues), Times.Once);
        }

        [Theory]
        [InlineData(1000, new bool[] { false, true, true, false })]
        public async Task ReadDiscretes_ShouldWork(ushort startingAddress, bool[] expectedValues)
        {
            var storageMock = new Mock<IDevicePointStorage<bool>>();

            storageMock.Setup(s => s.ReadPoints(startingAddress, (ushort)expectedValues.Length))
                .Returns(expectedValues);

            var implementation = new ReadDiscreteInputsImplementation(loggerFactory, storageMock.Object);

            var request = new ReadDiscreteInputsRequest(startingAddress, (ushort)expectedValues.Length);

            var response = await implementation.ProcessAsync(request, default);

            var unpacked = response.Unpack((ushort)expectedValues.Length);

            unpacked.ShouldBe(expectedValues);         
        }
    }
}

