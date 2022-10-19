namespace NModbus.Endian
{
    public static class EndianWriterExtensions
    {
        public static void Write(this EndianWriter writer, ushort[] values)
        {
            foreach (var value in values)
            {
                writer.Write(value);
            }
        }
    }
}
