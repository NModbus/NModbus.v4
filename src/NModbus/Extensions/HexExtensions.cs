namespace NModbus.Extensions
{
    public static class HexExtensions
    {
        public static string ToHex(this byte value)
        {
            return $"0x{value:X2}";
        }
    }
}
