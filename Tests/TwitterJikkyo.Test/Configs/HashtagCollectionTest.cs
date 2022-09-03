using Kzrnm.EventHandlerHistory;
using System.Collections.Specialized;
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

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-10)]
    [InlineData(-100)]
    [InlineData(-1000)]
    public void NotPositiveMaxSize(int maxSize)
    {
        maxSize
            .Invoking(s => new HashtagCollection(s))
            .Should()
            .ThrowExactly<ArgumentOutOfRangeException>();
    }

    [Theory]
    [InlineData(1)]
    [InlineData(10)]
    [InlineData(100)]
    [InlineData(1000)]
    public void Count(int maxSize)
    {
        var h = new HashtagCollection(maxSize);
        for (int i = 0; i < maxSize; i++)
        {
            h.Should().HaveCount(i);
            h.Add($"{i}");
        }
        h.Should().HaveCount(maxSize);
        h.Add("over");
        h.Should()
            .HaveCount(maxSize)
            .And
            .Equal(Enumerable.Range(0, maxSize).Select(i => i.ToString()).ToArray());
    }

    [Fact]
    public void AddUnique()
    {
        var h = new HashtagCollection();
        var handler = new CollectionChangedHistory(h);
        handler.Clear();
        h.AddUnique("1");
        h.Should().Equal("1");
        handler.Should().HaveCount(1);
        handler.First.Should().BeEquivalentTo(
            new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, "1", 0)
        ); ;

        handler.Clear();
        h.AddUnique("2");
        h.Should().Equal("2", "1");
        handler.Should().HaveCount(1);
        handler.First.Should().BeEquivalentTo(
            new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, "2", 0)
        );

        handler.Clear();
        h.AddUnique("1");
        h.Should().Equal("1", "2");
        handler.Should().HaveCount(1);
        handler.First.Should().BeEquivalentTo(
            new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Move, "1", 0, 1)
        ); ;
    }
}