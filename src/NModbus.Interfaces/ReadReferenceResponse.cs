namespace NModbus.Interfaces
{
    public struct ReadReferenceResponse
    {
        public ReferenceType Type { get; }

        public ushort[] RegisterData { get; }
    }
}
