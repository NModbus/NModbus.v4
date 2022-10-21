namespace NModbus
{
    /// <summary>
    /// Supported function codes
    /// </summary>
    public static class ModbusFunctionCodes
    {
        /// <summary>
        /// Has the Most Significant Bit Set. Or this with a function code to set the error bit.
        /// </summary>
        public const byte ErrorMask = 0b10000000;

        public const byte ReadCoils = 0x01;

        public const byte ReadDiscreteInputs = 0x02;

        public const byte ReadHoldingRegisters = 0x03;

        public const byte ReadInputRegisters = 0x04;

        public const byte WriteSingleCoil = 0x05;

        public const byte WriteSingleRegister = 0x06;

        public const byte Diagnostics = 0x08;

        public const byte WriteMultipleCoils = 0x0F;

        public const byte WriteMultipleRegisters = 0x10;

        public const byte ReadFileRecord = 0x14;

        public const byte WriteFileRecord = 0x15;

        public const byte MaskWriteRegister = 0x16;

        public const byte ReadWriteMultipleRegisters = 0x17;

        public const byte ReadFifoQueue = 0x18;

        /// <summary>
        /// Sets the error bit.
        /// </summary>
        /// <param name="functionCode"></param>
        /// <returns></returns>
        public static byte SetErrorBit(byte functionCode)
        {
            return (byte)(functionCode | ErrorMask);
        }

        /// <summary>
        /// Removes the error bit.
        /// </summary>
        /// <param name="functionCode"></param>
        /// <returns></returns>
        public static byte RemoveErrorBit(byte functionCode)
        {
            return (byte)(functionCode & 0b01111111);
        }

        /// <summary>
        /// Determines if the error bit is set.
        /// </summary>
        /// <param name="functionCode"></param>
        /// <returns></returns>
        public static bool IsErrorBitSet(byte functionCode)
        {
            return (functionCode & ErrorMask) > 0;
        }
    }
}
