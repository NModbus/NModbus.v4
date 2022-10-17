using NModbus.EndianTools;
using NModbus.Functions;

namespace NModbus.Messages
{
    /// <summary>
    /// Handles parsing/creating request and response messages.
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    public abstract class ModbusMessageSerializer<TRequest, TResponse> : IModbusMessageSerializer<TRequest, TResponse>
    {
        public async Task<byte[]> SerializeRequestAsync(TRequest request, CancellationToken cancellationToken = default)
        {
            using var stream = new MemoryStream();

            var writer = new EndianWriter(stream, Endianness.BigEndian);

            await SerializeRequestCoreAsync(request, writer, cancellationToken);

            return stream.ToArray();
        }

        public async Task<byte[]> SerializeResponseAsync(TResponse response, CancellationToken cancellationToken = default)
        {
            using var stream = new MemoryStream();

            var writer = new EndianWriter(stream, Endianness.BigEndian);

            await SeserializeResponseCoreAsync(response, writer, cancellationToken);

            return stream.ToArray();
        }

        public async Task<TRequest> DeserializeRequestAsync(byte[] data, CancellationToken cancellationToken = default)
        {
            using var stream = new MemoryStream(data);

            var reader = new EndianReader(stream, Endianness.BigEndian);

            return await DeserializeRequestCoreAsync(reader, cancellationToken);
        }

        public async Task<TResponse> DeserializeResponseAsync(byte[] data, CancellationToken cancellationToken = default)
        {
            using var stream = new MemoryStream(data);

            var reader = new EndianReader(stream, Endianness.BigEndian);

            return await DeserializeResponseCoreAsync(reader, cancellationToken);
        }

        protected abstract Task SerializeRequestCoreAsync(TRequest request, EndianWriter writer, CancellationToken cancellationToken);

        protected abstract Task SeserializeResponseCoreAsync(TResponse response, EndianWriter writer, CancellationToken cancellationToken);

        protected abstract Task<TRequest> DeserializeRequestCoreAsync(EndianReader reader, CancellationToken cancellationToken);

        protected abstract Task<TResponse> DeserializeResponseCoreAsync(EndianReader reader, CancellationToken cancellationToken);
    }
}
