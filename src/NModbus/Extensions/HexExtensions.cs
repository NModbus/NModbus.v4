namespace NModbus.Extensions
{
    /// <summary>
    /// Extensions to aid in the display of binary values in hex format.
    /// </summary>
    public static class HexExtensions
    {
        /// <summary>
        /// Displays a single byte in hex format (e.g. '0x0F')
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToHex(this byte value)
        {
            return $"0x{value:X2}";
        }
    }
}
