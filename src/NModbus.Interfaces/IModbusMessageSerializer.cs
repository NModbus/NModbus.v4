namespace NModbus.Functions
{
    public interface IModbusMessageSerializer<TRequest, TResponse>
    {
        Task<TRequest> DeserializeRequestAsync(byte[] data, CancellationToken cancellationToken = default);
        Task<TResponse> DeserializeResponseAsync(byte[] data, CancellationToken cancellationToken = default);
        Task<byte[]> SerializeRequestAsync(TRequest request, CancellationToken cancellationToken = default);
        Task<byte[]> SerializeResponseAsync(TResponse response, CancellationToken cancellationToken = default);
    }
}