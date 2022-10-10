namespace NModbus.Interfaces
{
    public interface IModbusFunctionImplementation<TRequest, TResponse>
    {
        Task<TResponse> ProcessAsync(TRequest request, CancellationToken cancellationToken);
    }
}
