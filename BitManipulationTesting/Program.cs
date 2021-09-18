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
            int runs = 50;

            List<double> a = new List<double>();
            Stopwatch sa = new Stopwatch();
            for (int j = 0; j < runs; j++)
            {
                sa.Restart();
                BitWriter bw = new BitWriter(2000000, 1024);
                for (int i = 0; i < 1000000; i++) bw.Put(4294967295UL);
                byte[] da = bw.Assemble();
                BitReader br = new BitReader(da);
                for (int i = 0; i < 1000000; i++) br.GetULong();
                sa.Stop();
                double time = sa.ElapsedTicks / 10000000d;
                a.Add(time);
            }
            double sua = 0;
            foreach (double t in a) sua += t;
            Console.WriteLine($"BitManipulator avg time: {sua / runs}ms");

            List<double> b = new List<double>();
            Stopwatch sb = new Stopwatch();
            for (int j = 0; j < runs; j++)
            {
                sb.Restart();
                NetDataWriter ndw = new NetDataWriter();
                for (int i = 0; i < 1000000; i++) ndw.Put(4294967295);
                byte[] db = ndw.CopyData();
                NetDataReader ndr = new NetDataReader(db);
                for (int i = 0; i < 1000000; i++) ndr.GetUInt();
                sb.Stop();
                double time = sa.ElapsedTicks / 10000000d;
                b.Add(time);
            }
            double sub = 0;
            foreach (double t in b) sub += t;
            Console.WriteLine($"LiteNetLib avg time: {sub / runs}ms");
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
