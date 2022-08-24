using System.Collections.Immutable;

namespace Kzrnm.RectCapturer;
internal static class ImmutableUtility
{
    public static ImmutableArray<T> GetOrEmpty<T>(this ImmutableArray<T> array)
        => array.IsDefault ? ImmutableArray<T>.Empty : array;
}
