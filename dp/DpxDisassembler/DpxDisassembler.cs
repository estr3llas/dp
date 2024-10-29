using System.Text;

namespace dp.DpxDisassembler;

using dp.DpxInstructionSet;
public class DpxDisassembler
{
    public Guid DpxBytesToGuid(byte[] bytes) => new Guid(bytes);

    private static int _index = 0;

    public byte[] DpxDisassembleHeader(byte[] bytecode)
    {
        var header = new byte[4];

        for (; _index < 4; _index++)
        {
            header[_index] = bytecode[_index];
        }

        return header;
    }

    public StringBuilder? DpxDisassembleBody(byte[] bytecode)
    {
        if (bytecode.Length < 2)
        {
            Console.WriteLine("[-] Invalid Dependency Expression: Too short.");
            return null;
        }

        var disassembled = new StringBuilder();

        for (;_index < bytecode.Length; _index++)
        {

            var opcode = (Opcodes)bytecode[_index];
            string mnemonic;

            switch (opcode)
            {
                case Opcodes.PUSH:
                    mnemonic = DpxInstructionSet.MnemonicFromOpcode(Opcodes.PUSH);
                    disassembled.AppendLine(mnemonic);

                    var byteArray = new byte[16];
                    Array.Copy(bytecode, _index + 1, byteArray, 0, 16);
                    var guid = DpxBytesToGuid(byteArray);
                    disassembled.AppendLine(guid.ToString());

                    _index += 16;
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
                    throw new ArgumentOutOfRangeException($"Unknown opcode: {_index}");
            }
        }

        return disassembled;
    }
}