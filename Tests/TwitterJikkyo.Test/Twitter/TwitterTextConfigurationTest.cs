namespace Kzrnm.TwitterJikkyo.Twitter;
public class TwitterTextConfigurationTest
{
    public static TheoryData<string, int> TextData = new()
    {
        { "", 280},
        { "a", 279},
        { "あ", 278},
        { "🙋🙋", 276},
        { "http://www.google.com/", 257},
        { "https://www.google.com/search", 257},
        { "https://www.google.com/search", 257},
        { "https://www.google.com/search aあ", 253 },
    };


    [Theory]
    [MemberData(nameof(TextData))]
    public void GetRemainingTweetTextLength(string text, int length)
    {
        TweetTextStringInfo.GetRemainingTweetTextLength(text).Should().Be(length);
    }
}
