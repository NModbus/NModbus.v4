﻿using NModbus.Interfaces;

namespace NModbus.Transport.IP
{
    /// <summary>
    /// This container will exist for the lifetime of a single TCP request and optional response.
    /// </summary>
    public interface IPerRequestStreamContainer : IAsyncDisposable
    {
        /// <summary>
        /// Gets the underlying stream
        /// </summary>
        IModbusStream Stream { get; }
    }
}
