using NModbus.Interfaces;

namespace NModbus.Transport.Serial;

public static class RtuSerializer
{
    public static byte[] Serialize(IModbusDataUnit message)
    {
        throw new NotImplementedException(); 
    }

    public static IModbusDataUnit Deserialize(byte[] source)
    {
        throw new NotImplementedException(); 
    }
}


