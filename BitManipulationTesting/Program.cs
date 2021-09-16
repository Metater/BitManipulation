using System;
using System.IO;
using BitManipulation;

namespace BitManipulationTesting
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            BitWriter bitWriter = new BitWriter(8);
            /*
            bitWriter.WriteByte(127, 6);
            bitWriter.WriteBit(true);
            bitWriter.WriteBit(true);
            bitWriter.WriteInt(0xff, 24);
            bitWriter.WriteBit(true);
            bitWriter.WriteByte(0x55, 7);
            bitWriter.WriteByte(0x55);
            bitWriter.WriteByte(0x55);
            bitWriter.WriteByte(0x55);
            */
            bitWriter.WriteInt(1431655765, 31);
            bitWriter.WriteBit(true);
            Console.WriteLine(bitWriter.scratch);
            //bitWriter.WriteLong(1L << 63, 64);
            byte[] data = bitWriter.Assemble();
            foreach (byte b in data)
            {
                Console.WriteLine(b);
            }
            Console.WriteLine("--------------------------");
            BitWriter.PrintBits(data);
            BitReader bitReader = new BitReader(data);
            Console.WriteLine(bitReader.ReadInt(31));
            Console.WriteLine(bitReader.ReadBit());
            //Console.WriteLine(bitReader.ReadInt());
            /*
            for (int i = 0; i < data.Length; i++)
            {
                string line = "";
                for (int j = 0; j < 8; j++)
                {
                    if (bitReader.ReadBit())
                        line += "1";
                    else
                        line += "0";
                }
                Console.WriteLine(line);
            }
            */
        }
    }
}
