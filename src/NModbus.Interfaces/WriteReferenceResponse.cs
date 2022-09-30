namespace NModbus.Interfaces
{
    public struct WriteReferenceResponse
    {
        public ReferenceType Type { get; }

        public ushort FileNumber { get; }

        public ushort RecordNumber { get; }

        public ushort[] RegisterData { get; }
    }
}
