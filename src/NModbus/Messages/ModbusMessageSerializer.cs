using NModbus.Endian;
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
        public byte[] SerializeRequest(TRequest request)
        {
            using var writer = new EndianWriter(Endianness.BigEndian);

            SerializeRequestCore(request, writer);

            return writer.ToArray();
        }

        public byte[] SerializeResponse(TResponse response)
        {
            using var writer = new EndianWriter(Endianness.BigEndian);

            SeserializeResponseCore(response, writer);

            return writer.ToArray();
        }

        public TRequest DeserializeRequest(byte[] data)
        {
            var reader = new EndianReader(data, Endianness.BigEndian);

            return DeserializeRequestCore(reader);
        }

        public TResponse DeserializeResponse(byte[] data)
        {
            var reader = new EndianReader(data, Endianness.BigEndian);

            return DeserializeResponseCore(reader);
        }

        protected abstract void SerializeRequestCore(TRequest request, EndianWriter writer);

        protected abstract void SeserializeResponseCore(TResponse response, EndianWriter writer);

        protected abstract TRequest DeserializeRequestCore(EndianReader reader);

        protected abstract TResponse DeserializeResponseCore(EndianReader reader);
    }
}
