using System.Collections.Generic;
using Windows.Win32;

namespace WindowScreenshot;

public class NaturalComparer : IComparer<string>
{
    public static NaturalComparer Default { get; } = new NaturalComparer();
    public int Compare(string? x, string? y) => (x, y) switch
    {
        (null, null) => 0,
        (null, _) => -1,
        (_, null) => 1,
        _ => PInvoke.StrCmpLogical(x, y),
    };
}