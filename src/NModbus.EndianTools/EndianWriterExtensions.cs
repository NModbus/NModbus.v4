namespace NModbus.EndianTools
{
    public static class EndianWriterExtensions
    {
        public static async Task WriteAsync(this EndianWriter writer, ushort[] values, CancellationToken cancellationToken = default)
        {
            foreach(var value in values)
            {
                await writer.WriteAsync(value, cancellationToken);
            }
        }
    }
}
