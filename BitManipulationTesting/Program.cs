using System;
using BitManipulation;

namespace BitManipulationTesting
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            BitWriterOld bitWriter = new BitWriterOld();
            bitWriter.ProvisionBytes(1);
            bitWriter.WriteByte(127, 6);
            byte[] data = bitWriter.Assemble();
            BitWriterOld.PrintByteArray(data);
        }
    }
}
