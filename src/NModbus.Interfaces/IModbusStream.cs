namespace NModbus.Interfaces
{
    /// <summary>
    /// Represents a stream resource.
    /// </summary>
    public  interface IModbusStream : IAsyncDisposable
    {
        /// <summary>
        /// Gets or sets the number of milliseconds before a timeout occurs when a read operation does not finish.
        /// </summary>
        int ReceiveTimeout { get; set; }

        /// <summary>
        /// Gets or sets the number of milliseconds before a timeout occurs when a write operation does not finish.
        /// </summary>
        int SendTimeout { get; set; }

        /// <summary>
        /// Reads a number of bytes from the input buffer.
        /// </summary>
        /// <param name="buffer">The byte array to write the collected data to.</param>
        /// <param name="offset">The offset in the buffer array to begin writing.</param>
        /// <param name="count">The number of bytes to read.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>The number of bytes that were actually read.</returns>
        Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken = default);

        /// <summary>
        /// Writes the specified number of bytes.
        /// </summary>
        /// <param name="buffer">Byte array containing the data to write.</param>
        /// <param name="offset">The offset at which to begin writing</param>
        /// <param name="count">The number of bytes to write.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken = default);
    }
}
