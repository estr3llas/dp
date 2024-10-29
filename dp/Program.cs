using System;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Text;

using dp.DpxInstructionSet;
using dp.DpxFileHandler;
using dp.DpxDisassembler;

using McMaster.Extensions.CommandLineUtils;

namespace dp
{

    [Command(Name = "dp", Description = "Parse/patch UEFI module's dependency expressions")]
    [HelpOption("-h")]
    public class Program
    {
        static Task<int> Main(string[] args) => CommandLineApplication.ExecuteAsync<Program>(args);

        [Argument(0, Description = "The depex file to be processed.")]
        private string filename { get; }

        [Option("-d", Description = "Disassemble the given depex")]
        public bool OptionDisassemble { get; }

        private async Task<int> OnExecuteAsync(CommandLineApplication app, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(filename))
            {
                app.ShowHelp();
                return 0;
            }

            if (OptionDisassemble)
            {
                Console.WriteLine("[+] Disassembled output: \n");

                var disasm = new DpxDisassembler.DpxDisassembler();
                var disassembled = disasm.DpxDisassembleBytecode(FileHandler.DpxReadFile(filename));
                Console.Write(disassembled);
                return 0;
            }

            return 0;
        }

    }
}
