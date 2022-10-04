using NModbus.EndianTools;

namespace NModbus.Functions
{
    public abstract class ModbusMessageFactory<TRequest, TResponse>
    {
        public async Task<byte[]> GetDataFromRequestAsync(TRequest request, CancellationToken cancellationToken = default)
        {
            using var stream = new MemoryStream();

            var writer = new EndianWriter(stream, Endianness.BigEndian);

            await CreateRequestCoreAsync(request, writer, cancellationToken);

            return stream.ToArray();
        }

        public async Task<byte[]> GetDataFromResponseAsync(TResponse response, CancellationToken cancellationToken = default)
        {
            using var stream = new MemoryStream();

            var writer = new EndianWriter(stream, Endianness.BigEndian);

            await CreateResponseCoreAsync(response, writer, cancellationToken);

            return stream.ToArray();
        }

        public async Task<TRequest> GetRequestFromDataAsync(byte[] data, CancellationToken cancellationToken = default)
        {
            using var stream = new MemoryStream(data);

            var reader = new EndianReader(stream, Endianness.BigEndian);

            return await GetRequestFromDataCoreAsync(reader, cancellationToken);
        }

        public async Task<TResponse> GetResponseFromDataAsync(byte[] data, CancellationToken cancellationToken = default)
        {
            using var stream = new MemoryStream(data);

            var reader = new EndianReader(stream, Endianness.BigEndian);

            return await GetResponseFromDataCoreAsync(reader, cancellationToken);
        }

        public abstract Task CreateRequestCoreAsync(TRequest request, EndianWriter writer, CancellationToken cancellationToken);

        public abstract Task CreateResponseCoreAsync(TResponse response, EndianWriter writer, CancellationToken cancellationToken);

        public abstract Task<TRequest> GetRequestFromDataCoreAsync(EndianReader reader, CancellationToken cancellationToken);

        public abstract Task<TResponse> GetResponseFromDataCoreAsync(EndianReader reader, CancellationToken cancellationToken);
    }
}
