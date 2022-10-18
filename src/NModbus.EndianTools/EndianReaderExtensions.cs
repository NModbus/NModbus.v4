namespace NModbus.EndianTools
{
    public static class EndianReaderExtensions
    {
        public static ushort[] ReadUInt16Array(this EndianReader reader, int length)
        {
            var values = new ushort[length];

            for(var index = 0; index < length; index++)
            {
                values[index] = reader.ReadUInt16();
            }

            return values;
        }
    }
}
