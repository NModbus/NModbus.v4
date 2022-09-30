namespace NModbus.Interfaces
{
    /// <summary>
    /// Initiates communication and makes requests of server device.
    /// </summary>
    public interface IModbusClient
    {
        /// <summary>
        /// 0x01
        /// </summary>
        /// <param name="serverAddress"></param>
        /// <param name="startingAddress"></param>
        /// <param name="quantity"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool[]> ReadCoilsAsync(
            byte serverAddress, 
            ushort startingAddress, 
            ushort quantity,  
            CancellationToken cancellationToken = default);

        /// <summary>
        /// 0x02
        /// </summary>
        /// <param name="serverAddress"></param>
        /// <param name="startingAddress"></param>
        /// <param name="quantity"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool[]> ReadDiscreteInputsAsync(
            byte serverAddress,
            ushort startingAddress,
            ushort quantity,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// 0x03
        /// </summary>
        /// <param name="serverAddress"></param>
        /// <param name="startingAddress"></param>
        /// <param name="quantity"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ushort[]> ReadHoldingRegistersAsync(
            byte serverAddress,
            ushort startingAddress,
            ushort quantity,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// 0x05
        /// </summary>
        /// <param name="serverAddress"></param>
        /// <param name="startingAddress"></param>
        /// <param name="value"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task WriteSingleCoilAsync(
            byte serverAddress,
            ushort startingAddress,
            bool value,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// 0x06
        /// </summary>
        /// <param name="serverAddress"></param>
        /// <param name="startingAddress"></param>
        /// <param name="value"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task WriteSingleRegisterAsync(
            byte serverAddress,
            ushort startingAddress,
            ushort value,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// 0x0f
        /// </summary>
        /// <param name="serverAddress"></param>
        /// <param name="startingAddress"></param>
        /// <param name="values"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task WriteMultipleCoilsAsync(
            byte serverAddress,
            ushort startingAddress,
            bool[] values,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// 0x10
        /// </summary>
        /// <param name="serverAddress"></param>
        /// <param name="startingAddress"></param>
        /// <param name="values"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task WriteMultipleRegistersAsync(
            byte serverAddress,
            ushort startingAddress,
            ushort[] values,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// 0x14
        /// </summary>
        /// <param name="serverAddress"></param>
        /// <param name="requests"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ReadReferenceResponse[]> ReadFileRecordAsync(
            byte serverAddress,
            ReadReferenceRequest[] requests,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// 0x15
        /// </summary>
        /// <param name="serverAddress"></param>
        /// <param name="requests"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<WriteReferenceResponse[]> WriteFileRecordAsync(
            byte serverAddress,
            WriteReferenceRequest[] requests,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// 0x15
        /// </summary>
        /// <param name="serverAddress"></param>
        /// <param name="startingAddress"></param>
        /// <param name="andMask"></param>
        /// <param name="orMask"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task MaskWriteRegisterAsync(
            byte serverAddress,
            ushort startingAddress,
            ushort andMask,
            ushort orMask,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// 0x17
        /// </summary>
        /// <param name="serverAddress"></param>
        /// <param name="startingAddress"></param>
        /// <param name="values"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ushort[]> ReadWriteMultipleRegistersAsync(
            byte serverAddress,
            ushort startingAddress,
            ushort[] values,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// 0x18
        /// </summary>
        /// <param name="serverAddress"></param>
        /// <param name="fifoPointerAddress"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ushort[]> ReadFifoQueueAsync(
            byte serverAddress,
            ushort fifoPointerAddress,
            CancellationToken cancellationToken = default);  
    }
}
