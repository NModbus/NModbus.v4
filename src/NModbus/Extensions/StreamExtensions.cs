namespace NModbus.Extensions
{
    public static class StreamExtensions
    {
        /// <summary>
        /// Fills the buffer from the stream.
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="buffer"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>True if the read was successful, false if no data was returned (indiciating that the connection wsa closed).</returns>
        /// <exception cref="IOException"></exception>
        public static async Task<bool> TryReadBufferAsync(this Stream stream, byte[] buffer, CancellationToken cancellationToken = default)
        {
            var totalRead = 0;

            while (totalRead < buffer.Length)
            {
                var read = await stream.ReadAsync(buffer, totalRead, buffer.Length - totalRead, cancellationToken);

                if (read == 0)
                    return false;

                totalRead+= read;
            }

            return true;
        }

        public static bool TryReadBuffer(this Stream stream, byte[] buffer)
        {
            var totalRead = 0;

            while (totalRead < buffer.Length)
            {
                var read = stream.Read(buffer, totalRead, buffer.Length - totalRead);

                if (read == 0)
                    return false;

                totalRead += read;
            }

            return true;
        }
    }
}
