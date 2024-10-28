using System;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Reflection;

namespace dp.DpxFile;

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
        //the NUL terminator is part of MAX_PATH https://msdn.microsoft.com/en-us/library/aa365247.aspx#maxpath
        //
        MaxPathLength--;
    }


    public static bool IsPathWithinLimits(string fullPathAndFilename)
    {
        return fullPathAndFilename.Length <= MaxPathLength;
    }

}
public class DpxFile : PathHelper
{
    public MemoryMappedFile DpxMapFile(string filename)
    {

        if (string.IsNullOrWhiteSpace(filename)) throw new ArgumentNullException($"[-] {filename} is null or whitespace.");

        if (!IsPathWithinLimits(filename)) throw new PathTooLongException($"[-] {filename} is too long.");

        //
        // No need to check for the file's actual existence.
        // For FileMode.Open, a FileNotFoundException exception is thrown if the file does not exist.
        //
        return MemoryMappedFile.CreateFromFile(filename, FileMode.Open);
    }
}