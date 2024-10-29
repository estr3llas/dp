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
            var disasm = new DpxDisassembler.DpxDisassembler();

            var disassembled = disasm.DpxDisassembleBytecode(FileHandler.DpxReadFile(args[1]));

            Console.Write(disassembled);

        }
    }
}
