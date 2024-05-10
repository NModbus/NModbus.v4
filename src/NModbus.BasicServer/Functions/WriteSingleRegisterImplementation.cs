﻿using Microsoft.Extensions.Logging;
using NModbus.BasicServer.Interfaces;
using NModbus.Interfaces;
using NModbus.Messages;

namespace NModbus.BasicServer.Functions
{
    public class WriteSingleRegisterImplementation : IModbusFunctionImplementation<WriteSingleRegisterRequest, WriteSingleRegisterResponse>
    {
        private readonly IDevicePointStorage<ushort> _storage;

        public WriteSingleRegisterImplementation(ILoggerFactory loggerFactory, IDevicePointStorage<ushort> storage)
        {
            if (loggerFactory is null) throw new ArgumentNullException(nameof(loggerFactory));
            _storage = storage ?? throw new ArgumentNullException(nameof(storage));
        }

        public Task<WriteSingleRegisterResponse> ProcessAsync(WriteSingleRegisterRequest request, CancellationToken cancellationToken)
        {
            _storage.WritePoints(request.Address, new ushort[] { request.Value });

            return Task.FromResult(new WriteSingleRegisterResponse(request.Address, request.Value));
        }
    }
}
