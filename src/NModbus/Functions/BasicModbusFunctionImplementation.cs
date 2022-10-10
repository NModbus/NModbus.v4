using NModbus.Interfaces;

namespace NModbus.Functions
{
    /// <summary>
    /// Provides a trivial implementation of <see cref="IModbusFunctionImplementation{TRequest, TResponse}" /> using a func.
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    public class BasicModbusFunctionImplementation<TRequest, TResponse> : IModbusFunctionImplementation<TRequest, TResponse>
    {
        private readonly Func<TRequest, CancellationToken, Task<TResponse>> func;

        public BasicModbusFunctionImplementation(Func<TRequest, CancellationToken, Task<TResponse>> func)
        {
            this.func = func ?? throw new ArgumentNullException(nameof(func));
        }

        public async Task<TResponse> ProcessAsync(TRequest request, CancellationToken cancellationToken)
        {
            return await func(request, cancellationToken);
        }
    }
}
