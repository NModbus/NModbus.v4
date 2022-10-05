namespace NModbus.Functions
{
    public interface IModbusFunctionImplementation<TRequest, TResponse>
    {
        Task<TResponse> ProcessAsync(TRequest request, CancellationToken cancellationToken);
    }
}
