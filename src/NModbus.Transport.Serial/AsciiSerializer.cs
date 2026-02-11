using NModbus.Interfaces;

namespace NModbus.Transport.Serial;

public static class AsciiSerializer
{
    private const char START= ':';
    private const string END = "\r\n";

    //Message format is:
    // Start  | Address | Function | Data                   | LRC     | End              |
    // ----   | ------- | -------- | -------                | ------- | -----------      |
    // 1 char | 2 chars | 2 chars  | 0 up to 2x252 char(s)  | 2 chars | 2 chars (CR,LF)  |

    public static string Serialize(IModbusDataUnit message)
    {
        //Create a buffer with enough room for this part of the message.
        var buffer = new byte[1 + message.ProtocolDataUnit.Length];

        buffer[0] = message.UnitIdentifier;

        Array.Copy(message.ProtocolDataUnit.ToArray(), 0, buffer, 0, message.ProtocolDataUnit.Length);

        var lrc = buffer.CalculateLrc();

        return START + $"{string.Join("", buffer.Select(b => b.ToString("X2")))}{lrc:X2}" + END;
    }

    public static IModbusDataUnit Deserialize(string source)
    {
        throw new NotImplementedException();
    }
}


