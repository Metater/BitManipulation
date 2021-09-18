﻿using System;
using System.Diagnostics;
using System.IO;
using BitManipulation;

namespace BitManipulationTesting
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BitWriter bw = new BitWriter(2, 2);
            bw.Put(0xff, 7);
            bw.Put(0x55, 8);
            byte[] data = bw.Assemble();
            PrintBits(data);
            BitReader br = new BitReader(data);
            for (int i = 0; i < 7; i++) br.GetBool();
            Console.WriteLine(br.GetByte());
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
