namespace NModbus.Messages
{
    public struct ReadReferenceResponse
    {
        public ReferenceType Type { get; }

        public ushort[] RegisterData { get; }
    }
}
