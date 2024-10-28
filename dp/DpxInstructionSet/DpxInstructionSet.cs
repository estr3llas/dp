namespace dp.DpxInstructionSet
{
    public enum Opcodes : uint
    {
        //
        // Pushes a Boolean value onto the stack. 
        // If the GUID is present in the handle database, then a TRUE is pushed onto the stack. 
        // If the GUID is not present in the handle database, then a FALSE is pushed onto the stack. 
        // The test for the GUID in the handle database may be performed with the Boot Service LocatePpi() .       
        //
        PUSH = 0x2,

        //
        // Pops two Boolean operands off the stack, performs a Boolean AND operation between the two operands, and pushes the result back onto the stack.
        //
        AND = 0x3,

        //
        // Pops two Boolean operands off the stack, performs a Boolean OR operation between the two operands, and pushes the result back onto the stack.
        //
        OR = 0x4,

        //
        // Pops a Boolean operands off the stack, performs a Boolean NOT operation on the operand, and pushes the result back onto the stack.
        //
        NOT = 0x5,

        //
        // Pushes a Boolean TRUE onto the stack.
        //
        TRUE = 0x6,

        //
        // Pushes a Boolean FALSE onto the stack.
        // 
        FALSE = 0x7,

        //
        // Pops the final result of the dependency expression evaluation off the stack and exits the dependency expression evaluator.
        // 
        END = 0x8
    }

    internal class DpxInstructionSet
    {
        public List<Instruction> _instructions =
        [
            new Instruction
            {
                Mnemonic = MnemonicFromOpcode(Opcodes.PUSH), NumberOfOperands = 1
            },
            new Instruction
            {
                Mnemonic = MnemonicFromOpcode(Opcodes.AND), NumberOfOperands = 0
            },
            new Instruction
            {
                Mnemonic = MnemonicFromOpcode(Opcodes.OR), NumberOfOperands = 0
            },
            new Instruction
            {
                Mnemonic = MnemonicFromOpcode(Opcodes.NOT), NumberOfOperands = 0
            },
            new Instruction
            {
                Mnemonic = MnemonicFromOpcode(Opcodes.TRUE), NumberOfOperands = 0
            },
            new Instruction
            {
                Mnemonic = MnemonicFromOpcode(Opcodes.FALSE), NumberOfOperands = 0
            },
            new Instruction
            {
                Mnemonic = MnemonicFromOpcode(Opcodes.END), NumberOfOperands = 0
            },

        ];

        public static string MnemonicFromOpcode(Opcodes opcode)
        {
            return opcode switch
            {
                Opcodes.PUSH => "PUSH",
                Opcodes.AND => "AND",
                Opcodes.OR => "OR",
                Opcodes.NOT => "NOT",
                Opcodes.TRUE => "TRUE",
                Opcodes.FALSE => "FALSE",
                Opcodes.END => "END",
                _ => throw new ArgumentOutOfRangeException(nameof(opcode), $"[-] Invalid Opcode {opcode}")
            };
        }

    }

    public class Instruction
    {
        public required string Mnemonic { get; init; }
        public required uint NumberOfOperands { get; init; }
    }
}
