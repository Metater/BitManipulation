using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace BitManipulation
{
    public class BitWriter
    {
        private ulong scratch = 0;
        private int scratchIndex = 0;

        private int wordIndex = 0;
        private uint[] buffer = new uint[0];

        private readonly int collisionProvision;

        // faster byte[] write
        // manual config of sign, mantissa, and exponent, prob too far

        public BitWriter(int provisionWords = 0, int collisionProvision = 1)
        {
            ProvisionWords(provisionWords);
            if (collisionProvision < 1) collisionProvision = 1;
            this.collisionProvision = collisionProvision;
        }

        public void ProvisionWords(int count)
        {
            Array.Resize(ref buffer, buffer.Length + count);
        }

        #region PutMethods
        public void Put(bool value)
        {
            CheckScratch();
            if (value) scratch |= 1UL << scratchIndex;
            scratchIndex++;
        }

        public void Put(byte value, int bits = 8)
        {
            CheckScratch();
            scratch |= ((ulong)value << 64 - bits) >> (64 - bits) - scratchIndex;
            scratchIndex += bits;
        }
        public void Put(sbyte value, int bits = 8)
        {
            CheckScratch();
            scratch |= ((ulong)value << 64 - bits) >> (64 - bits) - scratchIndex;
            scratchIndex += bits;
        }

        public void Put(char value)
        {
            CheckScratch();
            scratch |= ((ulong)value << 64 - 8) >> (64 - 8) - scratchIndex;
            scratchIndex += 8;
        }

        public void Put(ushort value, int bits = 16)
        {
            CheckScratch();
            scratch |= ((ulong)value << 64 - bits) >> (64 - bits) - scratchIndex;
            scratchIndex += bits;
        }
        public void Put(short value, int bits = 16)
        {
            CheckScratch();
            scratch |= ((ulong)value << 64 - bits) >> (64 - bits) - scratchIndex;
            scratchIndex += bits;
        }

        public void Put(uint value, int bits = 32)
        {
            CheckScratch();
            scratch |= ((ulong)value << 64 - bits) >> (64 - bits) - scratchIndex;
            scratchIndex += bits;
        }
        public void Put(int value, int bits = 32)
        {
            CheckScratch();
            scratch |= ((ulong)value << 64 - bits) >> (64 - bits) - scratchIndex;
            scratchIndex += bits;
        }

        public void Put(ulong value, int bits = 64)
        {
            if (bits <= 32)
            {
                Put((uint)value, bits);
                return;
            }
            Put((uint)value);
            Put((uint)(value >> 32), bits - 32);
        }
        public void Put(long value, int bits = 64)
        {
            if (bits <= 32)
            {
                Put((uint)value, bits);
                return;
            }
            Put((uint)value);
            Put((uint)(value >> 32), bits - 32);
        }

        public void Put(float value)
        {
            ConverterHelperFloat ch = new ConverterHelperFloat { i = value };
            Put(ch.o);
        }
        public void Put(double value)
        {
            ConverterHelperDouble ch = new ConverterHelperDouble { i = value };
            Put(ch.o);
        }

        public void Put(string value, int lengthBits)
        {
            if (string.IsNullOrEmpty(value))
            {
                Put(0, lengthBits);
                return;
            }

            byte[] values = Encoding.UTF8.GetBytes(value);
            Put(values.Length, lengthBits);

            for (int i = 0; i < values.Length; i++)
                Put(values[i]);
        }
        #endregion PutMethods

        #region PutArrayMethods
        public void PutArray(bool[] value, int lengthBits)
        {
            Put(value.Length, lengthBits);
            for (int i = 0; i < value.Length || i < (1 << lengthBits); i++)
                Put(value[i]);
        }

        public void PutArray(byte[] value, int valueBits, int lengthBits)
        {
            Put(value.Length, lengthBits);
            for (int i = 0; i < value.Length || i < (1 << lengthBits); i++)
                Put(value[i], valueBits);
        }
        public void PutArray(sbyte[] value, int valueBits, int lengthBits)
        {
            Put(value.Length, lengthBits);
            for (int i = 0; i < value.Length || i < (1 << lengthBits); i++)
                Put(value[i], valueBits);
        }

        public void PutArray(char[] value, int lengthBits)
        {
            Put(value.Length, lengthBits);
            for (int i = 0; i < value.Length || i < (1 << lengthBits); i++)
                Put(value[i]);
        }

        public void PutArray(ushort[] value, int valueBits, int lengthBits)
        {
            Put(value.Length, lengthBits);
            for (int i = 0; i < value.Length || i < (1 << lengthBits); i++)
                Put(value[i], valueBits);
        }
        public void PutArray(short[] value, int valueBits, int lengthBits)
        {
            Put(value.Length, lengthBits);
            for (int i = 0; i < value.Length || i < (1 << lengthBits); i++)
                Put(value[i], valueBits);
        }

        public void PutArray(uint[] value, int valueBits, int lengthBits)
        {
            Put(value.Length, lengthBits);
            for (int i = 0; i < value.Length || i < (1 << lengthBits); i++)
                Put(value[i], valueBits);
        }
        public void PutArray(int[] value, int valueBits, int lengthBits)
        {
            Put(value.Length, lengthBits);
            for (int i = 0; i < value.Length || i < (1 << lengthBits); i++)
                Put(value[i], valueBits);
        }

        public void PutArray(ulong[] value, int valueBits, int lengthBits)
        {
            Put(value.Length, lengthBits);
            for (int i = 0; i < value.Length || i < (1 << lengthBits); i++)
                Put(value[i], valueBits);
        }
        public void PutArray(long[] value, int valueBits, int lengthBits)
        {
            Put(value.Length, lengthBits);
            for (int i = 0; i < value.Length || i < (1 << lengthBits); i++)
                Put(value[i], valueBits);
        }

        public void PutArray(float[] value, int lengthBits)
        {
            Put(value.Length, lengthBits);
            for (int i = 0; i < value.Length || i < (1 << lengthBits); i++)
                Put(value[i]);
        }
        public void PutArray(double[] value, int lengthBits)
        {
            Put(value.Length, lengthBits);
            for (int i = 0; i < value.Length || i < (1 << lengthBits); i++)
                Put(value[i]);
        }

        public void PutArray(string[] value, int valueLengthBits, int lengthBits)
        {
            Put(value.Length, lengthBits);
            for (int i = 0; i < value.Length || i < (1 << lengthBits); i++)
                Put(value[i], valueLengthBits);
        }
        #endregion PutArrayMethods

        public byte[] Assemble()
        {
            CheckScratch(true);
            int trim = 0;
            int maxBufferIndex = wordIndex - 1;
            uint last = buffer[maxBufferIndex];
            if (last < 1 << 8) trim = 3;
            else if (last < 1 << 16) trim = 2;
            else if (last < 1 << 24) trim = 1;
            byte[] data = new byte[(wordIndex * 4) - trim];
            for (int i = 0; i < maxBufferIndex; i++)
                WriteLittleEndian(data, i * 4, buffer[i]);
            for (int i = 0; i < 4 - trim; i++)
                data[(maxBufferIndex * 4) + i] = (byte)(buffer[maxBufferIndex] >> (i * 8));
            return data;
        }

        private void CheckScratch(bool force = false)
        {
            if (!force) if (scratchIndex < 32) return;
            if (buffer.Length < wordIndex + 1) ProvisionWords(collisionProvision);
            buffer[wordIndex] = (uint)scratch;
            scratch >>= 32;
            scratchIndex -= 32;
            wordIndex++;
        }

        private static void WriteLittleEndian(byte[] buffer, int offset, uint data)
        {
            buffer[offset] = (byte)(data);
            buffer[offset + 1] = (byte)(data >> 8);
            buffer[offset + 2] = (byte)(data >> 16);
            buffer[offset + 3] = (byte)(data >> 24);
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct ConverterHelperDouble
        {
            [FieldOffset(0)]
            public ulong o;

            [FieldOffset(0)]
            public double i;
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct ConverterHelperFloat
        {
            [FieldOffset(0)]
            public int o;

            [FieldOffset(0)]
            public float i;
        }
    }
}
