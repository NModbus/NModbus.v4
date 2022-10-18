namespace NModbus.Functions
{
    public interface IModbusMessageSerializer<TRequest, TResponse>
    {
        TRequest DeserializeRequest(byte[] data);

        TResponse DeserializeResponse(byte[] data);

        byte[] SerializeRequest(TRequest request);

        byte[] SerializeResponse(TResponse response);
    }
}