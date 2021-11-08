using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using BitManipulation;
using LiteNetLib.Utils;

namespace BitManipulationTesting
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BitReader reader = new BitReader();
            byte[] test = new byte[8000000];
            reader.SetSource(test);

            Stopwatch sw = new Stopwatch();
            sw.Start();
            for (int i = 0; i < 8000; i++)
            {
                reader.GetBool();
            }
            sw.Stop();
            Console.WriteLine(sw.ElapsedTicks / 10000d);

            sw.Reset();

            NetDataReader reader2 = new NetDataReader();
            reader2.SetSource(test);
            sw.Start();
            for (int i = 0; i < 8000; i++)
            {
                reader2.GetBool();
            }
            sw.Stop();
            Console.WriteLine(sw.ElapsedTicks / 10000d);

            /*
            BitWriter bw = new BitWriter();
            bw.Put((int)((4.5f * 256f) + 8192), 14);
            bw.Put((int)((3.5f * 256f)), 10);
            bw.Put((int)((0 * 256f) + 8192), 14);
            bw.Put((byte)(180 / 1.41176470588f));
            bw.Put((byte)(((0 + 90f) / 0.70588235294f)));
            byte[] data = bw.Assemble();
            PrintBits(data);

            BitReader br = new BitReader(data);
            int x = br.GetInt(14);
            int y = br.GetInt(10);
            int z = br.GetInt(14);
            int r = br.GetByte(8);
            int p = br.GetByte(8);
            Console.WriteLine(x);
            Console.WriteLine(y);
            Console.WriteLine(z);
            Console.WriteLine(r);
            Console.WriteLine(p);
            Console.WriteLine("EEE");
            float xPos = (x - 8192) / 256f;
            float yPos = (y) / 256f;
            float zPos = (z - 8192) / 256f;
            float rot = (r * 1.41176470588f);
            float pitch = (p * 0.70588235294f) - 90f;
            Console.WriteLine(xPos);
            Console.WriteLine(yPos);
            Console.WriteLine(zPos);
            Console.WriteLine(rot);
            Console.WriteLine(pitch);
            */
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
