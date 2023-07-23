namespace NModbus.Interfaces
{

    /// <summary>
    /// The MODBUS protocol defines a simple protocol data unit (PDU) independent of the 
    /// underlying communication layers.
    /// </summary>
    public class ProtocolDataUnit
    {
        /// <summary>
        /// Constructs a <see cref="ProtocolDataUnit"/> from a raw binary buffer.
        /// </summary>
        /// <param name="buffer"></param>
        public ProtocolDataUnit(ReadOnlyMemory<byte> buffer)
        {
            FunctionCode = buffer[..1].ToArray()[0];
            Data = buffer[1..];
        }

        /// <summary>
        /// Constructs a <see cref="ProtocolDataUnit"/> from a function code and data payload.
        /// </summary>
        /// <param name="functionCode"></param>
        /// <param name="data"></param>
        public ProtocolDataUnit(byte functionCode, byte[] data)
        {
            FunctionCode = functionCode;
            Data = data;
        }

        /// <summary>
        /// The function code.
        /// </summary>
        public byte FunctionCode { get; }
        
        /// <summary>
        /// The data part of this <see cref="ProtocolDataUnit"/>.
        /// </summary>
        public ReadOnlyMemory<byte> Data { get; }

        /// <summary>
        /// Gets the full length of this <see cref="ProtocolDataUnit"/>.
        /// </summary>
        public int Length => Data.Length + 1;

        /// <summary>
        /// Gets a byte array representation of the PDU.
        /// </summary>
        /// <returns></returns>
        public byte[] ToArray()
        {
            var buffer = new byte[Data.Length + 1];

            buffer[0] = FunctionCode;

            Array.Copy(Data.ToArray(), 0, buffer, 1, Data.Length);

            return buffer;
        }
    }
}
