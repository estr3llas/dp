using dp.DpxInstructionSet;

namespace dp
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var instructionSet = new InstructionSet();

            Console.WriteLine($"{"Mnemonic:",-10} {"Operands:",-8}");

            foreach (var instruction in instructionSet._instructions)
            {
                Console.WriteLine($"{instruction.Mnemonic, -10} {instruction.NumberOfOperands, -8}");
            }
        }
    }
}
