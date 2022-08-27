using System.Text.Json;

namespace Kzrnm.TwitterJikkyo.Configs;

public class HashtagCollectionTest
{
    public static readonly TheoryData<string, int, string[]> SerializationData = new()
    {
        {
            @"{""MaxTagNum"":200,""Hashtags"":[""foo"",""bar"",""baz""]}", 200, new string[]{ "foo", "bar", "baz" }
        },
        {
            @"{""MaxTagNum"":150,""Hashtags"":[""foo"",""bar"",""baz""]}", 150, new string[]{ "foo", "bar", "baz" }
        },
    };

    [Theory]
    [MemberData(nameof(SerializationData))]
    public void Serialize(string json, int maxTagNum, string[] tags)
    {
        JsonSerializer.Serialize(new HashtagCollection(tags, maxTagNum))
            .Should().Be(json);
    }


    [Theory]
    [MemberData(nameof(SerializationData))]
    public void Deserialize(string json, int maxTagNum, string[] tags)
    {
        var h = JsonSerializer.Deserialize<HashtagCollection>(json);
        Assert.NotNull(h);
        h.Should().NotBeNull();
        h.MaxTagNum.Should().Be(maxTagNum);
        h.Should().Equal(tags);
    }
}