using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using System.Net.Security;
using System.Text;

namespace dp.DpxDisassembler;

using DpxInstructionSet;
using System.IO;

public class DpxDisassembler
{
    public static Guid DpxBytesToGuid(byte[] bytes) => new(bytes);

    //
    // This will be our static index.
    // It gets increased first by DpxDisassembleHeader to extract the depex's header.
    // Then, increased by DpxDisassembleBody to actually disassemble the depex's body.
    //
    private static int _index = 0;
    private static readonly char[] Separator = new[] { ',', '\n' };

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

    public static string DpxCheckIfGuidIsKnown(Guid guid)
    {
        //
        // Extract GUIDs database from the executable's resources.
        //
        var guidsResource = Properties.Resources.guids.Split(Separator, StringSplitOptions.RemoveEmptyEntries);
        var upperGuid = guid.ToString().ToUpper();

        //
        // Iterate over the whole GUID database and compare our argument.
        // TODO: optimize this
        //
        for (var i = 0; i < guidsResource.Length - 1; i++)
        {
            if (upperGuid.Equals(guidsResource[i]))
            {
                return guidsResource[i+1].Trim();
            }
        }

        return string.Empty;
    }

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
                //
                // PUSH has 1 operand.
                //
                case Opcodes.PUSH:
                    //
                    // Disassemble the mnemonic
                    //
                    mnemonic = DpxInstructionSet.MnemonicFromOpcode(Opcodes.PUSH);

                    //
                    // Then, for the next 16 bytes, extract its operand.
                    //
                    var byteArray = new byte[16];
                    Array.Copy(bytecode, _index + 1, byteArray, 0, 16);
                    var guid = DpxBytesToGuid(byteArray);
                    
                    //
                    // Check if the operand (GUID) is Known by comparing with UEFITool's guids.csv
                    //
                    var isKnownGuid = DpxCheckIfGuidIsKnown(guid);

                    //
                    // If GUID is known, append it as:
                    // PUSH  EfiPeiMemoryDiscoveredPpiGuid            (f894643d-c449-42d1-8ea8-85bdd8c65bde)
                    // If not known, append as:
                    // PUSH  1a266768-fd43-4e18-a88a-35c794c3910e
                    //
                    if (!string.IsNullOrEmpty(isKnownGuid))
                    {
                        disassembled.AppendLine($"{mnemonic,-5} {isKnownGuid,-40} ({guid})");
                    }
                    else
                    {
                        disassembled.AppendLine($"{mnemonic,-5} {guid}");
                    }


                    //
                    // Increase current index by 16 bytes (size of GUID, in bytes)
                    // Our index now points to the next instruction.
                    //
                    _index += 16;
                    break;


                //
                // Other cases are standard, meaning no operands at all.
                //
                case Opcodes.AND:
                case Opcodes.OR:
                case Opcodes.NOT:
                case Opcodes.TRUE:
                case Opcodes.FALSE:
                case Opcodes.END:
                    mnemonic = DpxInstructionSet.MnemonicFromOpcode(opcode);
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