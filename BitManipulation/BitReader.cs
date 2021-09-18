using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace BitManipulation
{
    public class BitReader
    {
        private byte[] data;

        private int position = 0;

        // faster byte[] read: Jump to next byte, read all stuff to buffer, then read all bytes
        // switching data length sizes in arrays with bools or 2,3,4 bit numbers
        // Make the data buffer a uint buffer, could improve performance?

        public BitReader(byte[] data)
        {
            this.data = data;
        }

        public void SkipBits(int count)
        {
            position += count;
        }

        #region GetMethods
        public bool GetBool()
        {
            bool value = ((data[GetIndex()] >> GetRemainder()) & 1) != 0;
            position++;
            return value;
        }

        public byte GetByte(int bits = 8)
        {
            byte value = 0;
            int remainingBits = bits;
            while (remainingBits > 0)
            {
                int bitsRead = 8 - GetRemainder();
                if (bitsRead > remainingBits) bitsRead = remainingBits;
                value |= (byte)(GetNextByte() << bits - remainingBits);
                remainingBits -= bitsRead;
                position += bitsRead;
            }
            return value;
        }
        public sbyte GetSByte(int bits = 8)
        {
            sbyte value = 0;
            int remainingBits = bits;
            while (remainingBits > 0)
            {
                int bitsRead = 8 - GetRemainder();
                if (bitsRead > remainingBits) bitsRead = remainingBits;
                value |= (sbyte)(GetNextByte() << bits - remainingBits);
                remainingBits -= bitsRead;
                position += bitsRead;
            }
            return value;
        }

        public char GetChar()
        {
            char value = char.MinValue;
            int remainingBits = 8;
            while (remainingBits > 0)
            {
                int bitsRead = 8 - GetRemainder();
                if (bitsRead > remainingBits) bitsRead = remainingBits;
                value |= (char)(GetNextByte() << 8 - remainingBits);
                remainingBits -= bitsRead;
                position += bitsRead;
            }
            return value;
        }

        public ushort GetUShort(int bits = 16)
        {
            ushort value = 0;
            int remainingBits = bits;
            while (remainingBits > 0)
            {
                int bitsRead = 8 - GetRemainder();
                if (bitsRead > remainingBits) bitsRead = remainingBits;
                value |= (ushort)(GetNextByte() << bits - remainingBits);
                remainingBits -= bitsRead;
                position += bitsRead;
            }
            return value;
        }
        public short GetShort(int bits = 16)
        {
            short value = 0;
            int remainingBits = bits;
            while (remainingBits > 0)
            {
                int bitsRead = 8 - GetRemainder();
                if (bitsRead > remainingBits) bitsRead = remainingBits;
                value |= (short)(GetNextByte() << bits - remainingBits);
                remainingBits -= bitsRead;
                position += bitsRead;
            }
            return value;
        }

        public uint GetUInt(int bits = 32)
        {
            uint value = 0;
            int remainingBits = bits;
            while (remainingBits > 0)
            {
                int bitsRead = 8 - GetRemainder();
                if (bitsRead > remainingBits) bitsRead = remainingBits;
                value |= (uint)(GetNextByte() << bits - remainingBits);
                remainingBits -= bitsRead;
                position += bitsRead;
            }
            return value;
        }
        public int GetInt(int bits = 32)
        {
            int value = 0;
            int remainingBits = bits;
            while (remainingBits > 0)
            {
                int bitsRead = 8 - GetRemainder();
                if (bitsRead > remainingBits) bitsRead = remainingBits;
                value |= GetNextByte() << bits - remainingBits;
                remainingBits -= bitsRead;
                position += bitsRead;
            }
            return value;
        }

        public ulong GetULong(int bits = 64)
        {
            ulong value = 0;
            int remainingBits = bits;
            while (remainingBits > 0)
            {
                int bitsRead = 8 - GetRemainder();
                if (bitsRead > remainingBits) bitsRead = remainingBits;
                value |= ((ulong)GetNextByte()) << bits - remainingBits;
                remainingBits -= bitsRead;
                position += bitsRead;
            }
            return value;
        }
        public long GetLong(int bits = 64)
        {
            long value = 0;
            int remainingBits = bits;
            while (remainingBits > 0)
            {
                int bitsRead = 8 - GetRemainder();
                if (bitsRead > remainingBits) bitsRead = remainingBits;
                value |= ((long)GetNextByte()) << bits - remainingBits;
                remainingBits -= bitsRead;
                position += bitsRead;
            }
            return value;
        }

        public float GetFloat()
        {
            BitWriter.ConverterHelperFloat ch = new BitWriter.ConverterHelperFloat { o = GetInt() };
            return ch.i;
        }
        public double GetDouble()
        {
            BitWriter.ConverterHelperDouble ch = new BitWriter.ConverterHelperDouble { o = GetULong() };
            return ch.i;
        }

        public string GetString(int lengthBits)
        {
            byte[] value = GetByteArray(8, lengthBits);
            return Encoding.UTF8.GetString(value);
        }
        #endregion GetMethods

        #region ArrayGetMethods
        public bool[] GetBoolArray(int lengthBits)
        {
            int length = GetInt(lengthBits);
            bool[] value = new bool[length];
            for (int i = 0; i < length; i++)
                value[i] = GetBool();
            return value;
        }

        public byte[] GetByteArray(int valueBits, int lengthBits)
        {
            int length = GetInt(lengthBits);
            byte[] value = new byte[length];
            for (int i = 0; i < length; i++)
                value[i] = GetByte(valueBits);
            return value;
        }
        public sbyte[] GetSByteArray(int valueBits, int lengthBits)
        {
            int length = GetInt(lengthBits);
            sbyte[] value = new sbyte[length];
            for (int i = 0; i < length; i++)
                value[i] = GetSByte(valueBits);
            return value;
        }

        public char[] GetCharArray(int lengthBits)
        {
            int length = GetInt(lengthBits);
            char[] value = new char[length];
            for (int i = 0; i < length; i++)
                value[i] = GetChar();
            return value;
        }

        public ushort[] GetUShortArray(int valueBits, int lengthBits)
        {
            int length = GetInt(lengthBits);
            ushort[] value = new ushort[length];
            for (int i = 0; i < length; i++)
                value[i] = GetUShort(valueBits);
            return value;
        }
        public short[] GetShortArray(int valueBits, int lengthBits)
        {
            int length = GetInt(lengthBits);
            short[] value = new short[length];
            for (int i = 0; i < length; i++)
                value[i] = GetShort(valueBits);
            return value;
        }

        public uint[] GetUIntArray(int valueBits, int lengthBits)
        {
            int length = GetInt(lengthBits);
            uint[] value = new uint[length];
            for (int i = 0; i < length; i++)
                value[i] = GetUInt(valueBits);
            return value;
        }
        public int[] GetIntArray(int valueBits, int lengthBits)
        {
            int length = GetInt(lengthBits);
            int[] value = new int[length];
            for (int i = 0; i < length; i++)
                value[i] = GetInt(valueBits);
            return value;
        }

        public ulong[] GetULongArray(int valueBits, int lengthBits)
        {
            int length = GetInt(lengthBits);
            ulong[] value = new ulong[length];
            for (int i = 0; i < length; i++)
                value[i] = GetULong(valueBits);
            return value;
        }
        public long[] GetLongArray(int valueBits, int lengthBits)
        {
            int length = GetInt(lengthBits);
            long[] value = new long[length];
            for (int i = 0; i < length; i++)
                value[i] = GetLong(valueBits);
            return value;
        }

        public float[] GetFloatArray(int lengthBits)
        {
            int length = GetInt(lengthBits);
            float[] value = new float[length];
            for (int i = 0; i < length; i++)
                value[i] = GetFloat();
            return value;
        }
        public double[] GetDoubleArray(int lengthBits)
        {
            int length = GetInt(lengthBits);
            double[] value = new double[length];
            for (int i = 0; i < length; i++)
                value[i] = GetDouble();
            return value;
        }

        public string[] GetStringArray(int valueLengthBits, int lengthBits)
        {
            int length = GetInt(lengthBits);
            string[] value = new string[length];
            for (int i = 0; i < length; i++)
                value[i] = GetString(valueLengthBits);
            return value;
        }
        #endregion ArrayGetMethods

        private int GetIndex()
        {
            return position / 8;
        }
        private int GetRemainder()
        {
            return position % 8;
        }
        private byte GetNextByte()
        {
            if (data.Length <= GetIndex()) return 0x00;
            return (byte)(data[GetIndex()] >> GetRemainder());
        }
    }
}
