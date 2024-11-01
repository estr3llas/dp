using System;
using System.IO;
using System.IO.MemoryMappedFiles;
using dp.DpxUtil;

namespace dp.DpxFileHandler;
public static class FileHandler
{
    public static byte[]? DpxReadFile(string filename)
    {
        if (string.IsNullOrWhiteSpace(filename))
        {
            Console.WriteLine(@$"[-] {filename} is null or whitespace.");
            return null;
        }

        if (!PathHelper.IsPathWithinLimits(filename))
        {
            Console.WriteLine(@$"[-] {filename}'s path is too long.");
            return null;
        }

        return File.ReadAllBytes(filename);
    }
}
