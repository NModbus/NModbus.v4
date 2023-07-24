using Microsoft.Extensions.Logging;
using NModbus.BasicServer.Interfaces;
using NModbus.Interfaces;
using NModbus.Messages;

namespace NModbus.BasicServer.Functions
{
    public class MaskWriteRegisterImplementation
        : IModbusFunctionImplementation<MaskWriteRegisterRequest, MaskWriteRegisterResponse>
    {
        private readonly IDevicePointStorage<ushort> storage;

        public MaskWriteRegisterImplementation(ILoggerFactory loggerFactory, IDevicePointStorage<ushort> storage)
        {
            if (loggerFactory is null) throw new ArgumentNullException(nameof(loggerFactory));

            this.storage = storage ?? throw new ArgumentNullException(nameof(storage));
        }

        public Task<MaskWriteRegisterResponse> ProcessAsync(MaskWriteRegisterRequest request, CancellationToken cancellationToken)
        {
            var currentContents = storage.ReadPoints(request.ReferenceAddress, 1)[0];

            var newContents = MaskWrite(currentContents, request.AndMask, request.OrMask);

            storage.WritePoints(request.ReferenceAddress, new ushort[] { newContents });

            return Task.FromResult(new MaskWriteRegisterResponse(request.ReferenceAddress, request.AndMask, request.OrMask));
        }

        /// <summary>
        /// Mask write logic
        /// </summary>
        /// <param name="currentContents"></param>
        /// <param name="andMask"></param>
        /// <param name="orMask"></param>
        /// <returns></returns>
        /// <remarks>
        ///  From section 6.16 of Modbus Application Protocol Specification V1.1b3
        ///  The function’s algorithm is:
        ///     Result = (Current Contents AND And_Mask) OR(Or_Mask AND(NOT And_Mask))
        /// </remarks>
        public static ushort MaskWrite(ushort currentContents, ushort andMask, ushort orMask)
        {
            return (ushort)((currentContents & andMask) | (orMask & ((ushort)~andMask)));
        }
    }
}
