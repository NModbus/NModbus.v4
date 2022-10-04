namespace NModbus.Interfaces
{
    /// <summary>
    /// The MODBUS protocol defines a simple protocol data unit (PDU) independent of the 
    /// underlying communication layers.
    /// </summary>
    public class ProtocolDataUnit
    {
        public ProtocolDataUnit(ReadOnlyMemory<byte> buffer)
        {
            FunctionCode = buffer.Slice(0, 1).ToArray()[0];
            Data = buffer.Slice(1);
        }

        public byte FunctionCode { get; }
        
        public ReadOnlyMemory<byte> Data { get; }
    }
}
