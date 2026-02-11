using NModbus.Interfaces;

namespace NModbus.Transport.Serial.TransmissionModes;

public class TransmissionModeRtuSerializer : TransmissionModeSerializer
{
    public override IModbusDataUnit Deserialize(byte[] bytes)
    {
        throw new NotImplementedException();
    }

    public override byte[] Serialize(IModbusDataUnit message)
    {
        throw new NotImplementedException();
    }
}
