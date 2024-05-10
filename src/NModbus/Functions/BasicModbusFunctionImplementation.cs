﻿using NModbus.Interfaces;

namespace NModbus.Functions
{
    /// <summary>
    /// Provides a trivial implementation of <see cref="IModbusFunctionImplementation{TRequest, TResponse}" /> using a func.
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    public class BasicModbusFunctionImplementation<TRequest, TResponse> : IModbusFunctionImplementation<TRequest, TResponse>
    {
        private readonly Func<TRequest, CancellationToken, Task<TResponse>> _func;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="func"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public BasicModbusFunctionImplementation(Func<TRequest, CancellationToken, Task<TResponse>> func)
        {
            _func = func ?? throw new ArgumentNullException(nameof(func));
        }

        /// <inheritdoc/>
        public async Task<TResponse> ProcessAsync(TRequest request, CancellationToken cancellationToken)
        {
            return await _func(request, cancellationToken);
        }
    }
}
