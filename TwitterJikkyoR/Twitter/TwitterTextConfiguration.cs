using System.Collections.Immutable;

namespace Kzrnm.TwitterJikkyo.Twitter;

/// <summary>
/// version: 3
/// https://github.com/twitter/twitter-text/blob/30e2430d90cff3b46393ea54caf511441983c260/java/src/main/java/com/twitter/twittertext/TwitterTextConfiguration.java
/// </summary>
public record TwitterTextConfiguration(
    int MaxWeightedTweetLength,
    int Scale,
    int DefaultWeight,
    int TransformedURLLength,
    bool EmojiParsingEnabled,
    ImmutableArray<CodePointRange> Ranges
)
{
    public static DefaultTwitterTextConfiguration Default { get; } = new DefaultTwitterTextConfiguration();
    public virtual int CodePointToWeight(int code)
    {
        foreach (var r in Ranges.AsSpan())
        {
            if (r.Start <= code && code <= r.End)
            {
                return r.Weight;
            }
        }
        return DefaultWeight;
    }
}
public record CodePointRange(int Start, int End, int Weight);

public sealed record DefaultTwitterTextConfiguration() : TwitterTextConfiguration(
    MaxWeightedTweetLength: 280,
    Scale: 100,
    DefaultWeight: DefaultWeightConst,
    TransformedURLLength: 23,
    EmojiParsingEnabled: true,
    Ranges: ImmutableArray.Create(
        new CodePointRange(Start: 0, End: 4351, Weight: 100),
        new CodePointRange(Start: 8192, End: 8205, Weight: 100),
        new CodePointRange(Start: 8208, End: 8223, Weight: 100),
        new CodePointRange(Start: 8242, End: 8247, Weight: 100)
    )
)
{
    private const int DefaultWeightConst = 200;
    public override int CodePointToWeight(int code) => code switch
    {
        >= 0 and <= 4351 => 100,
        >= 8192 and <= 8205 => 100,
        >= 8208 and <= 8223 => 100,
        >= 8242 and <= 8247 => 100,
        _ => DefaultWeightConst,
    };
}