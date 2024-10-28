using System;
using System.IO;
using System.IO.MemoryMappedFiles;

using dp.DpxInstructionSet;
using dp.DpxFileHandler;

namespace dp
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var handler = new DpxFileHandler.DpxFileHandler();

            var _bytes = handler.DpxReadFile("C:\\Users\\OtavioPassos\\Downloads\\test.sct");

            foreach (var i in _bytes)
            {
                Console.Write($"{i:X} ");
            }
        }
    }
}
