using System.Text;

namespace dp.DpxDisassembler;

using DpxInstructionSet;

public class DpxDisassembler
{
    public Guid DpxBytesToGuid(byte[] bytes) => new(bytes);

    //
    // This will be our static index.
    // It gets increased first by DpxDisassembleHeader to extract the depex's header.
    // Then, increased by DpxDisassembleBody to actually disassemble the depex's body.
    //
    private static int _index = 0;

    //
    // The first byte of the depex's body MUST be a instruction
    //
    public bool DpxCheckValidBody(byte[] bytecode) => new[]
    {
        Opcodes.PUSH, 
        Opcodes.AND,
        Opcodes.OR, 
        Opcodes.NOT, 
        Opcodes.FALSE,
        Opcodes.TRUE
    }.Contains((Opcodes)bytecode[4]);

    public byte[] DpxDisassembleHeader(byte[] bytecode)
    {
        var header = new byte[4];

        //
        // Extract the depex's header.
        // (always the first 4 bytes)
        //
        for (; _index < 4; _index++)
        {
            header[_index] = bytecode[_index];
        }

        return header;
    }

    public StringBuilder? DpxDisassembleBody(byte[] bytecode)
    {
        //
        // The minimum body length is 2.
        //
        // Example:
        // TRUE
        // END
        //
        if (bytecode.Length < 2)
        {
            Console.WriteLine("[-] Invalid Dependency Expression: Too short.");
            return null;
        }

        var disassembled = new StringBuilder();

        //
        // At this point, "_index" holds the value 4.
        // We can assume the header was already extracted.
        //
        for (;_index < bytecode.Length; _index++)
        {

            var opcode = (Opcodes)bytecode[_index];
            string mnemonic;

            switch (opcode)
            {
                case Opcodes.PUSH:
                    //
                    // PUSH has 1 operand.
                    // Disassemble the mnemonic
                    //
                    mnemonic = DpxInstructionSet.MnemonicFromOpcode(Opcodes.PUSH);
                    disassembled.AppendLine(mnemonic);

                    //
                    // Then, for the next 16 bytes, extract its operand.
                    //
                    var byteArray = new byte[16];
                    Array.Copy(bytecode, _index + 1, byteArray, 0, 16);
                    var guid = DpxBytesToGuid(byteArray);
                    disassembled.AppendLine(guid.ToString());

                    //
                    // Increase current index by 16 bytes.
                    //
                    _index += 16;
                    break;


                //
                // Other cases are standard, meaning no operands at all.
                //
                case Opcodes.AND:
                    mnemonic = DpxInstructionSet.MnemonicFromOpcode(Opcodes.AND);
                    disassembled.AppendLine(mnemonic);
                    break;

                case Opcodes.OR:
                    mnemonic = DpxInstructionSet.MnemonicFromOpcode(Opcodes.OR);
                    disassembled.AppendLine(mnemonic);
                    break;

                case Opcodes.NOT:
                    mnemonic = DpxInstructionSet.MnemonicFromOpcode(Opcodes.NOT);
                    disassembled.AppendLine(mnemonic);
                    break;

                case Opcodes.TRUE:
                    mnemonic = DpxInstructionSet.MnemonicFromOpcode(Opcodes.TRUE);
                    disassembled.AppendLine(mnemonic);
                    break;

                case Opcodes.FALSE:
                    mnemonic = DpxInstructionSet.MnemonicFromOpcode(Opcodes.FALSE);
                    disassembled.AppendLine(mnemonic);
                    break;

                case Opcodes.END:
                    mnemonic = DpxInstructionSet.MnemonicFromOpcode(Opcodes.END);
                    disassembled.AppendLine(mnemonic);
                    break;

                //
                // Invalid opcode encountered.
                //
                default:
                    throw new ArgumentOutOfRangeException($"Unknown opcode: {_index}");
            }
        }

        return disassembled;
    }
}