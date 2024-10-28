namespace dp.DpxDisassembler;

using dp.DpxInstructionSet;

public class DpxDisassembler
{

    public Guid DpxBytesToGuid(byte[] bytes)
    {
        return new Guid(bytes);
    }
    public void DpxDisassembleBytecode(byte[] bytecode)
    {
        if (bytecode.Length < 2)
        {
            Console.WriteLine("[-] Invalid Dependency Expression: Too short.");
            return;
        }

    }
}