using Kzrnm.WindowScreenshot;
using System.Text.Json.Serialization;

namespace Kzrnm.TwitterJikkyo.Configs;

public record Hashtags(
    int MaxTagNum = 100,
    SelectorObservableCollection<string>? PresetHashtags = null
)
{
    [JsonPropertyName("Hashtags")]
    [JsonPropertyOrder(100)]
    public SelectorObservableCollection<string> PresetHashtags { get; init; } = PresetHashtags ?? new();
    public Hashtags() : this(PresetHashtags: null) { }
}
