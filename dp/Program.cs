using System;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Text;

using dp.DpxInstructionSet;
using dp.DpxFileHandler;
using dp.DpxDisassembler;

namespace dp
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var handler = new DpxFileHandler.DpxFileHandler();

            var _bytes = handler.DpxReadFile("C:\\Users\\__\\Downloads\\depex.dpx");

            var disasm = new DpxDisassembler.DpxDisassembler();

            StringBuilder disassembled = disasm.DpxDisassembleBytecode(_bytes);

            for (int i = 0; i < disassembled.Length; i++)
            {
                Console.Write($"{disassembled[i]}");
            }
        }
    }
}
