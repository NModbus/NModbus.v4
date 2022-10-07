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
        /// <returns></returns>
        /// <exception cref="IOException"></exception>
        public static async Task ReadBufferAsync(this Stream stream, byte[] buffer, CancellationToken cancellationToken = default)
        {
            var totalRead = 0;

            while (totalRead < buffer.Length)
            {
                var read = await stream.ReadAsync(buffer, totalRead, buffer.Length - totalRead, cancellationToken);

                if (read == 0)
                    throw new IOException("Read resulted in 0 bytes returned.");

                totalRead+= read;
            }
        }
    }
}
