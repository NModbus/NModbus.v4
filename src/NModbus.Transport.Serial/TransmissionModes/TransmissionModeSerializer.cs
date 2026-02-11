using NModbus.Interfaces;

namespace NModbus.Transport.Serial.TransmissionModes;

public abstract class TransmissionModeSerializer
{
    public abstract byte[] Serialize(IModbusDataUnit message);

    public abstract IModbusDataUnit Deserialize(byte[] bytes);
}