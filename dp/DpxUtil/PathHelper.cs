using System;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Reflection;

namespace dp.DpxUtil;
public class PathHelper
{
    //
    // https://stackoverflow.com/questions/23588944/better-to-check-if-length-exceeds-max-path-or-catch-pathtoolongexception
    //
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

    public static bool IsPathWithinLimits(string fullPathAndFilename) => fullPathAndFilename.Length >= MaxPathLength;
}