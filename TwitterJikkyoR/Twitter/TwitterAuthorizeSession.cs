using CoreTweet;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Kzrnm.TwitterJikkyo.Twitter;
public interface ITwitterAuthorizeSession
{
    Uri AuthorizeUri { get; }
    Task<Tokens> AuthorizeAsync(string pin, CancellationToken cancellationToken = default);
}
public class TwitterAuthorizeSession : ITwitterAuthorizeSession
{
    private readonly OAuth.OAuthSession session;
    public Uri AuthorizeUri => session.AuthorizeUri;
    private TwitterTokenService TwitterTokenService { get; }
    protected internal TwitterAuthorizeSession(TwitterTokenService twitterTokenService)
    {
        TwitterTokenService = twitterTokenService;
        session = OAuth.Authorize(twitterTokenService.ApiKey, twitterTokenService.ApiSecret);
    }
    public async Task<Tokens> AuthorizeAsync(string pin, CancellationToken cancellationToken = default)
    {
        var tokens = await session.GetTokensAsync(pin, cancellationToken).ConfigureAwait(false);
        TwitterTokenService.AddTokens(tokens);
        return tokens;
    }
}
