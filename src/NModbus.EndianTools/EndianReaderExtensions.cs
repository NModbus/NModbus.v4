namespace NModbus.EndianTools
{
    public static class EndianReaderExtensions
    {
        public static async Task<ushort[]> ReadUInt16ArrayAsync(this EndianReader reader, int length, CancellationToken cancellationToken = default)
        {
            var values = new ushort[length];

            for(var index = 0; index < length; index++)
            {
                values[index] = await reader.ReadUInt16Async(cancellationToken);
            }

            return values;
        }
    }
}
