﻿using NModbus.Extensions;

namespace NModbus.Endian
{
    public class EndianReader : IDisposable
    {
        private readonly Stream _stream;

        public EndianReader(byte[] source, Endianness endianness)
        {
            _stream = new MemoryStream(source);
            Endianness = endianness;
        }

        public Endianness Endianness { get; }

        public byte ReadByte()
        {
            var buffer = new byte[1];

            var numberRead = _stream.Read(buffer, 0, 1);

            if (numberRead != 1)
            {
                throw new InvalidOperationException($"Expected 1 bytes but got {numberRead} instead.");
            }

            return buffer[0];
        }

        public ushort ReadUInt16()
        {
            var bytes = ReadPrimitiveBytes(sizeof(ushort));

            return BitConverter.ToUInt16(bytes);
        }

        public byte[] ReadBytes(int length)
        {
            var buffer = new byte[length];

            if (!_stream.TryReadBuffer(buffer))
                return null;

            return buffer;
        }

        private byte[] ReadPrimitiveBytes(int count)
        {
            var buffer = new byte[count];

            var numberRead = _stream.Read(buffer, 0, count);

            if (numberRead != count)
            {
                throw new InvalidOperationException($"Expected {count} bytes but got {numberRead} instead.");
            }

            if (Endianness == Endianness.BigEndian)
            {
                Array.Reverse(buffer);
            }

            return buffer;
        }

        public void Dispose()
        {
            _stream.Dispose();
        }
    }
}
