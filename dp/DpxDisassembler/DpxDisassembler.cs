namespace dp.DpxDisassembler;

using dp.DpxInstructionSet;
using System.Text;

public class DpxDisassembler
{
    public Guid DpxBytesToGuid(byte[] bytes)
    {
        return new Guid(bytes);
    }

    public StringBuilder? DpxDisassembleBytecode(byte[] bytecode)
    {
        if (bytecode.Length < 2)
        {
            Console.WriteLine("[-] Invalid Dependency Expression: Too short.");
            return null;
        }

        var disassembled = new StringBuilder();

        for (var i = 0; i < bytecode.Length; i++)
        {
            var opcode = (Opcodes)bytecode[i];
            string mnemonic;

            switch (opcode)
            {
                case Opcodes.PUSH:
                    mnemonic = DpxInstructionSet.MnemonicFromOpcode(Opcodes.PUSH);
                    disassembled.AppendLine(mnemonic);

                    var byteArray = new byte[16];
                    Array.Copy(bytecode, i + 1, byteArray, 0, 16);
                    var guid = DpxBytesToGuid(byteArray);
                    disassembled.AppendLine(guid.ToString());

                    i += 16;
                    break;

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

                default:
                    throw new ArgumentOutOfRangeException($"Unknown opcode: {i}");
            }
        }

        return disassembled;
    }
}