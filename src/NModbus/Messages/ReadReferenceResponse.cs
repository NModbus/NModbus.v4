namespace NModbus.Messages
{
    public readonly struct ReadReferenceResponse
    {
        public ReferenceType Type { get; }

        public ushort[] RegisterData { get; }
    }
}
