using NModbus.Interfaces;
using System.IO.Ports;

namespace NModbus.Transport.Serial;

public class ModbusAsciiClientTransport(SerialPort serialPort) : IModbusClientTransport
{
    public async Task<IModbusDataUnit?> SendAndReceiveAsync(IModbusDataUnit message, CancellationToken cancellationToken = default)
    {
        var serialized = AsciiSerializer.Serialize(message);

        serialPort.Write(serialized);

        //TODO: Deal with timeouts
        var response = serialPort.ReadLine();

        if (response == null)
            throw new InvalidOperationException("The data received from the device was null.");

        var responseMessage = AsciiSerializer.Deserialize(response);

        return responseMessage;
    }

    public Task SendAsync(IModbusDataUnit message, CancellationToken cancellationToken = default)
    {
        var serialized = AsciiSerializer.Serialize(message);

        serialPort.Write(serialized);

        return Task.CompletedTask;
    }
    
    public ValueTask DisposeAsync()
    {
        return ValueTask.CompletedTask;
    }
}


