using System;

namespace BitManipulation
{
    public class BitWriterOld
    {
        private byte[] data = new byte[0];
        private int writePos = 0;

        private ulong scratch = 0;
        private int scratchIndex = 0;

        public void ProvisionBytes(int count)
        {
            byte[] newData = new byte[data.Length + count];
            Buffer.BlockCopy(data, 0, newData, 0, data.Length);
            data = newData;
        }
        public void ProvisionWords(int count)
        {
            ProvisionBytes(count * 4);
        }

        public void WriteBit(bool value)
        {
            CheckScratch();
            scratch = (ulong)(scratch >> 1);
            if (value) scratch |= (1UL) << 63;
            scratchIndex++;
        }
        public void WriteByte(byte value, int bits = 8)
        {
            CheckScratch();
            scratch = (scratch >> bits);
            scratch |= ((ulong)value) << 64 - bits;
            scratchIndex += bits;
        }
        public void WriteSByte(sbyte value, int bits = 8)
        {
            CheckScratch();
            scratch = (scratch >> bits);
            scratch |= ((ulong)value) << 64 - bits;
            scratchIndex += bits;
        }
        public void WriteUShort(ushort value, int bits = 16)
        {
            CheckScratch();
            scratch = (scratch >> bits);
            scratch |= ((ulong)value) << 64 - bits;
            scratchIndex += bits;
        }
        public void WriteShort(short value, int bits = 16)
        {
            CheckScratch();
            scratch = (scratch >> bits);
            scratch |= ((ulong)value) << 64 - bits;
            scratchIndex += bits;
        }
        public void WriteUInt(uint value, int bits = 32)
        {
            CheckScratch();
            scratch = (scratch >> bits);
            scratch |= ((ulong)value) << 64 - bits;
            scratchIndex += bits;
        }
        public void WriteInt(int value, int bits = 32)
        {
            CheckScratch();
            scratch = (scratch >> bits);
            scratch |= ((ulong)value) << 64 - bits;
            scratchIndex += bits;
        }
        public void WriteULong(ulong value, int bits = 64)
        {
            int value0 = (int)value;
            int value1 = (int)(value >> 32);
            if (bits <= 32)
                WriteInt(value0, bits);
            else
            {
                WriteInt(value0);
                WriteInt(value1, bits - 32);
            }
        }
        public void WriteLong(long value, int bits = 64)
        {
            int value0 = (int)value;
            int value1 = (int)(value >> 32);
            if (bits <= 32)
                WriteInt(value0, bits);
            else
            {
                WriteInt(value0);
                WriteInt(value1, bits - 32);
            }
        }

        public byte[] Assemble()
        {
            // Flipped wrong way, each byte is
            CheckScratch();
            int bitsInScratch = scratchIndex;
            if (bitsInScratch == 0) return TrimData();
            if (bitsInScratch <= 8)
            { // 1 byte
                scratch = (scratch >> (8 - bitsInScratch));
                data[writePos] = (byte)(scratch >> 56);
                writePos++;
            }
            else if (bitsInScratch <= 16)
            { // 2 bytes
                scratch = (scratch >> (8 - bitsInScratch));
                data[writePos] = (byte)(scratch >> 56);
                writePos++;
                data[writePos] = (byte)(scratch >> 48);
                writePos++;
            }
            else if (bitsInScratch <= 24)
            { // 3 bytes
                scratch = (scratch >> (8 - bitsInScratch));
                data[writePos] = (byte)(scratch >> 56);
                writePos++;
                data[writePos] = (byte)(scratch >> 48);
                writePos++;
                data[writePos] = (byte)(scratch >> 40);
                writePos++;
            }
            else
            { // 4 bytes
                scratch = (scratch >> (8 - bitsInScratch));
                data[writePos] = (byte)(scratch >> 56);
                writePos++;
                data[writePos] = (byte)(scratch >> 48);
                writePos++;
                data[writePos] = (byte)(scratch >> 40);
                writePos++;
                data[writePos] = (byte)(scratch >> 32);
                writePos++;
            }
            return TrimData();
        }

        private byte[] TrimData()
        {
            byte[] trimmedData = new byte[writePos];
            Buffer.BlockCopy(data, 0, trimmedData, 0, writePos);
            return trimmedData;
        }

        public void CheckScratch()
        {
            if (scratchIndex >= 32)
            {
                uint dump = (uint)(scratch >> scratchIndex);
                FastBitConverter.GetBytes(data, writePos, dump);
                scratch = (scratch >> 32);
                writePos += 4;
                scratchIndex -= 32;
            }
        }













        public void Print()
        {
            Console.WriteLine("Data:");
            int i = 1;
            string line = "";
            foreach (byte b in data)
            {
                line = b + "\t" + line;
                if (i % 8 == 0)
                {
                    Console.WriteLine(line);
                    line = "";
                }
                i++;
            }
            Console.WriteLine(line);
            Console.WriteLine();
            Console.WriteLine("Write Pos: " + writePos);
            Console.Write("Scratch: ");
            PrintBits(BitConverter.GetBytes(scratch));
            Console.WriteLine("Scratch index: " + scratchIndex);
        }
        public static void PrintByteArray(byte[] data)
        {
            Console.WriteLine("Data Byte Array:");
            int i = 1;
            string line = "";
            foreach (byte b in data)
            {
                line = b + "\t" + line;
                if (i % 8 == 0)
                {
                    Console.WriteLine(line);
                    line = "";
                }
                i++;
            }
            Console.WriteLine(line);
        }
        public static void PrintBits(byte[] bytes)
        {
            string print = "";

            int bitPos = 0;
            while (bitPos < 8 * bytes.Length)
            {
                int byteIndex = bitPos / 8;
                int offset = bitPos % 8;
                bool isSet = (bytes[byteIndex] & (1 << offset)) != 0;
                if (offset == 0) print = " " + print;
                if (isSet)
                {
                    print = "1" + print;
                }
                else
                {
                    print = "0" + print;
                }
                bitPos++;
            }
            Console.WriteLine(print);
        }
    }
}
