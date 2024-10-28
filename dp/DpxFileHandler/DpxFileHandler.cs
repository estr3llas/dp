using System;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Reflection;

namespace dp.DpxFileHandler;

//
// https://stackoverflow.com/questions/23588944/better-to-check-if-length-exceeds-max-path-or-catch-pathtoolongexception
//
public class PathHelper
{
    private static int MaxPathLength { get; set; }

    static PathHelper()
    {
        //
        // reflection
        //
        var maxPathField = typeof(Path).GetField(
            "MaxPath", 
            BindingFlags.Static | 
            BindingFlags.GetField | 
            BindingFlags.NonPublic
            );

        if (maxPathField == null)
        {
            MaxPathLength = 0;
            return;
        }

        //
        // invoke the field gettor, which returns 260
        //
        MaxPathLength = Convert.ToInt32(maxPathField.GetValue(null));

        //
        //the NULL terminator is part of MAX_PATH https://msdn.microsoft.com/en-us/library/aa365247.aspx#maxpath
        //
        MaxPathLength--;
    }


    public static bool IsPathWithinLimits(string fullPathAndFilename)
    {
        return fullPathAndFilename.Length >= MaxPathLength;
    }

}
public class DpxFileHandler
{
    public MemoryMappedFile? DpxMapFile(string filename)
    {
        if (string.IsNullOrWhiteSpace(filename))
        {
            Console.WriteLine($"[-] {filename} is null or whitespace.");
            return null;
        }

        if (!PathHelper.IsPathWithinLimits(filename))
        {
            Console.WriteLine($"[-] {filename}'s path is too long.");
            return null;
        }

        //
        // No need to check for the file's actual existence.
        //
        // https://learn.microsoft.com/en-us/dotnet/api/system.io.filemode?view=net-8.0#system-io-filemode-open
        // For FileMode.Open, a FileNotFoundException exception is thrown if the file does not exist.
        //
        return MemoryMappedFile.CreateFromFile(filename, FileMode.Open);
    }

    public byte[]? DpxReadFile(string filename)
    {
        if (string.IsNullOrWhiteSpace(filename))
        {
            Console.WriteLine($"[-] {filename} is null or whitespace.");
            return null;
        }

        if (!PathHelper.IsPathWithinLimits(filename))
        {
            Console.WriteLine($"[-] {filename}'s path is too long.");
            return null;
        }

        return File.ReadAllBytes(filename);
    }
}