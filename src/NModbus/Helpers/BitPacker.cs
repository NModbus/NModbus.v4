namespace NModbus.Helpers
{
    /// <summary>
    /// Utilities for packing and unpacking bits to and from byte arrays.
    /// </summary>
    public static class BitPacker
    {
        /// <summary>
        /// Unpacks the specified number of bits from a byte array.
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="bitCount"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static bool[] Unpack(this byte[] buffer, ushort bitCount)
        {
            if (buffer is null) throw new ArgumentNullException(nameof(buffer));

            var expectedNumberOfBytes = CalculateBytesToPackBits(bitCount);

            if (expectedNumberOfBytes != buffer.Length)
                throw new ArgumentOutOfRangeException(nameof(bitCount), $"{bitCount} bits were requested during this unpack operation but {buffer.Length} bytes were available instead of the expected {expectedNumberOfBytes}.");

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

        /// <summary>
        /// Packs the specified bits into the least possible number of bytes.
        /// </summary>
        /// <param name="bits"></param>
        /// <returns></returns>
        public static byte[] Pack(this bool[] bits)
        {
            var byteCount = CalculateBytesToPackBits(bits.Length);

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

        /// <summary>
        /// Determine the number of bytes required to pack the specified number of bits in.
        /// </summary>
        /// <param name="numberOfBits">The total number of bits to store in a byte array.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static int CalculateBytesToPackBits(int numberOfBits)
        {
            if (numberOfBits <= 0)
                throw new ArgumentOutOfRangeException(nameof(numberOfBits));
            
            var numberOfBytes = numberOfBits / 8;

            if (numberOfBits % 8 != 0)
                numberOfBytes++;

            return numberOfBytes;
        }
    }
}
