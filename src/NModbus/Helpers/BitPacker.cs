namespace NModbus.Helpers
{
    public static class BitPacker
    {
        public static bool[] Unpack(this byte[] buffer, ushort bitCount)
        {
            var bits = new bool[bitCount];

            var byteIndex = 0;
            byte mask = 0b00000001;

            for (var bitIndex = 0; bitIndex < bitCount; bitIndex++)
            {
                bits[bitIndex] = ((byte)(buffer[byteIndex] & mask)) == mask;

                mask <<= 1;

                if (mask == 0)
                {
                    byteIndex++;
                    mask = 0b00000001;
                }
            }

            return bits;
        }

        public static byte[] Pack(this bool[] bits)
        {
            var byteCount = CalculateBytesToPack(bits.Length);

            var bytes = new byte[byteCount];

            var byteIndex = 0;
            byte mask = 0b00000001;

            for(var bitIndex = 0; bitIndex < bits.Length; bitIndex++)
            {
                if (bits[bitIndex])
                {
                    bytes[byteIndex] |= mask;
                }

                mask <<= 1;

                if (mask == 0)
                {
                    byteIndex++;
                    mask = 0b00000001;
                }
            }

            return bytes;
        }

        public static int CalculateBytesToPack(int numberOfBits)
        {
            return (numberOfBits / 8) + 1;
        }
    }
}
