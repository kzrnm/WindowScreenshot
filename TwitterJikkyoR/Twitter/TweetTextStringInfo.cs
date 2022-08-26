using System.Text.RegularExpressions;

namespace Kzrnm.TwitterJikkyo.Twitter;
public static class TweetTextStringInfo
{
    private static readonly Regex UrlReg = new(@"https?://[-_.!~*'()a-zA-Z0-9;/?:@&=+$,%#]+", RegexOptions.Compiled);

    /// <summary>
    /// ツイートできる残り文字数を取得する
    /// 絵文字の判別は未実装
    /// </summary>
    /// <param name="text">判定する文字数</param>
    /// <returns>残り文字数</returns>
    public static int GetRemainingTweetTextLength(string text)
    {
        text = text.Replace("\r\n", "\n");
        text = UrlReg.Replace(text, new string('0', TwitterTextConfiguration.Default.TransformedURLLength));

        var totalWeight = 0;
        for (var i = 0; i < text.Length; i += char.IsSurrogatePair(text, i) ? 2 : 1)
        {
            var code = char.ConvertToUtf32(text, i);
            var weight = TwitterTextConfiguration.Default.CodePointToWeight(code);
            totalWeight += weight;
        }
        var tweetTextLength = totalWeight / TwitterTextConfiguration.Default.Scale;
        return TwitterTextConfiguration.Default.MaxWeightedTweetLength - tweetTextLength;
    }
}