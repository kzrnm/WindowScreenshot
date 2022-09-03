using CoreTweet;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Kzrnm.TwitterJikkyo.Twitter;
public class TwitterAuthorizeSession
{
    private readonly OAuth.OAuthSession session;
    public Uri AuthorizeUri => session.AuthorizeUri;
    private TwitterTokenService TwitterService { get; }
    protected internal TwitterAuthorizeSession(TwitterTokenService twitterService)
    {
        TwitterService = twitterService;
        session = OAuth.Authorize(twitterService.ApiKey, twitterService.ApiSecret);
    }
    public async Task<Tokens> AuthorizeAsync(string pin, CancellationToken cancellationToken = default)
    {
        var tokens = await session.GetTokensAsync(pin, cancellationToken).ConfigureAwait(false);
        TwitterService.AddTokens(tokens);
        return tokens;
    }
}
