using NModbus.Interfaces;
using NModbus.Transport.IP.Mbap;

namespace NModbus.Transport.IP
{
    internal static class ModbusIpMessageSerializer
    {
        internal static byte[] Serialize(this IModbusDataUnit message, ushort transactionIdentifier)
        {
            //Create the header
            var mbapHeader = MbapSerializer.SerializeMbapHeader(
                transactionIdentifier,
                (ushort)(message.ProtocolDataUnit.Length + 1),
                message.UnitIdentifier);

            //Create a buffer with enough room for the whole message.
            var buffer = new byte[mbapHeader.Length + message.ProtocolDataUnit.Length];

            //Copy the header in
            Array.Copy(mbapHeader, buffer, mbapHeader.Length);

            //Copy the PDU in
            Array.Copy(message.ProtocolDataUnit.ToArray(), 0, buffer, mbapHeader.Length, message.ProtocolDataUnit.Length);

            return buffer;
        }

        internal static ModbusIPMessage Deserialize(byte[] bytes)
        {
            if (bytes is null) throw new ArgumentNullException(nameof(bytes));

            if (bytes.Length < MbapSerializer.MbapHeaderLength)
                throw new InvalidOperationException($"The length of the buffer specified ({bytes.Length}) was less than the minimum {MbapSerializer.MbapHeaderLength} bytes necessary to store an MBAP header.");

            var mbapHeader = MbapSerializer.DeserializeMbapHeader(bytes);

            var expectedLength = mbapHeader.Length + mbapHeader.Length - 1;

            if (bytes.Length != expectedLength)
                throw new InvalidOperationException($"Expected {expectedLength} bytes but there were {bytes.Length}.");

            var pdu = new ProtocolDataUnit(new ArraySegment<byte>(bytes, MbapSerializer.MbapHeaderLength, bytes.Length - MbapSerializer.MbapHeaderLength));

            return new ModbusIPMessage(mbapHeader, pdu);
        }
    }
}
