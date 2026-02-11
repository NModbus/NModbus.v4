namespace NModbus.Transport.Serial;

public static class LrcExtensions
{
    public static byte CalculateLrc(this byte[] data)
    {
        byte accumulator = 0;

        foreach (var item in data)
        {
            accumulator += item;
        }

        return accumulator;
    }
}


