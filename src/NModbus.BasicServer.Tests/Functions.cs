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
        private readonly ILoggerFactory _loggerFactory;

        public Functions(ITestOutputHelper output)
        {
            _loggerFactory = LogFactory.Create(output);
        }

        [Theory]
        [InlineData(24, new ushort[] { 4, 5, 6 })]
        [InlineData(22, new ushort[] { 78 })]
        public async Task WriteHoldingRegisters_ShouldWork(ushort startingAddress, ushort[] values)
        {
            var storageMock = new Mock<IDevicePointStorage<ushort>>();

            var implementation = new WriteMultipleRegistersImplementation(_loggerFactory, storageMock.Object);

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

            var implementation = new ReadCoilsImplementation(_loggerFactory, storage);

            var request = new ReadCoilsRequest(startingAddress, (ushort)values.Length);

            var response = await implementation.ProcessAsync(request, default);

            response.Unpack((ushort)values.Length).ShouldBe(values);
        }

        [Theory]
        [InlineData(50, true)]
        public async Task WriteSingleCoil_ShouldWork(ushort outputAddress, bool outputValue)
        {
            var storageMock = new Mock<IDevicePointStorage<bool>>();

            var implementation = new WriteSingleCoilImplementation(_loggerFactory, storageMock.Object);

            var request = new WriteSingleCoilRequest(outputAddress, outputValue);

            await implementation.ProcessAsync(request, default);

            storageMock.Verify(m => m.WritePoints(outputAddress, new bool[] { outputValue }), Times.Once);
        }

        [Theory]
        [InlineData(100, new bool[] { true, false, false, false } )]
        public async Task WriteMultipleCoils_ShouldWork(ushort startingAddress, bool[] values)
        {
            var storageMock = new Mock<IDevicePointStorage<bool>>();

            var implementation = new WriteMultipleCoilsImplementation(_loggerFactory, storageMock.Object);

            var request = new WriteMultipleCoilsRequest(startingAddress, values);

            await implementation.ProcessAsync(request, default);

            storageMock.Verify(m => m.WritePoints(startingAddress, values), Times.Once);
        }

        [Theory]
        [InlineData(1000, new ushort[] { 1, 2, 3, 4, 5 }, 2000, new ushort[] { 500, 501, 503 })]
        public async Task ReadWriteMultipleRegisters_NonOverlappingShouldWork(ushort readStartingAddress, ushort[] readValues, ushort writeStartingAddress, ushort[] writeValues)
        {
            var storageMock = new Mock<IDevicePointStorage<ushort>>();

            storageMock.Setup(s => s.ReadPoints(readStartingAddress, (ushort)readValues.Length))
                .Returns(readValues);

            var implementation = new ReadWriteMultipleRegistersImplementation(_loggerFactory, storageMock.Object);

            var request = new ReadWriteMultipleRegistersRequest(readStartingAddress, (ushort)readValues.Length, writeStartingAddress, writeValues);

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

            var implementation = new ReadDiscreteInputsImplementation(_loggerFactory, storageMock.Object);

            var request = new ReadDiscreteInputsRequest(startingAddress, (ushort)expectedValues.Length);

            var response = await implementation.ProcessAsync(request, default);

            var unpacked = response.Unpack((ushort)expectedValues.Length);

            unpacked.ShouldBe(expectedValues);         
        }

        /// <summary>
        ///                    HX Binary
        ///                    -- ---------
        /// Current Contents = 12 0001 0010
        ///        And_Mask  = F2 1111 0010
        ///          Or_Mask = 25 0010 0101
        ///   (NOT And_Mask) = 0D 0000 1101
        ///           Result = 17 0001 0111
        /// </summary>
        /// <param name="currentContents"></param>
        /// <param name="andMask"></param>
        /// <param name="orMask"></param>
        /// <param name="expected"></param>
        [Theory]
        [InlineData(0b00010010, 0b11110010, 0b00100101, 0b00010111)]
        public void MaskWriteRegister(ushort currentContents, ushort andMask, ushort orMask, ushort expected)
        {
            MaskWriteRegisterImplementation.MaskWrite(currentContents, andMask, orMask)
                .ShouldBe(expected);
        }
    }
}

