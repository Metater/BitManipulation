using System;
using System.Collections.Generic;
using System.Text;

namespace BitManipulation
{
    public class BitReader
    {
        private byte[] data;
        private int readPos = 0;

        private ulong scratch = 0;
        private int scratchIndex = 0;

        public BitReader(byte[] data)
        {
            this.data = data;
        }

        public bool ReadBit()
        {
            CheckData();
            bool value = (scratch & (1UL << scratchIndex)) != 0;
            scratchIndex--;
            return value;
        }

        public int ReadInt(int bits = 32)
        {
            CheckData();
            int bitsToDrop = 32 - bits;
            int value = (int)scratch << bitsToDrop;
            value >>= bitsToDrop;
            scratch >>= bits;
            scratchIndex -= bits;
            return value;
        }

        private void CheckData()
        {
            if (scratchIndex <= 32)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (readPos >= data.Length) break;
                    scratch |= ((ulong)data[readPos + i] << scratchIndex);
                    scratchIndex += 8;
                }
                readPos += 4;
            }
        }
    }
}
