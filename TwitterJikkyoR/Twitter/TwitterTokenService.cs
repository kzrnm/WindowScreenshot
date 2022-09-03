using CoreTweet;
using Kzrnm.TwitterJikkyo.Configs;
using Kzrnm.TwitterJikkyo.Logic;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;

namespace Kzrnm.TwitterJikkyo.Twitter;
public class TwitterTokenService
{
    public TwitterTokenService(Secrets secrets, ConfigMaster configMaster, AesCrypt aesCrypt)
    {
        ApiKey = secrets.TwitterApiKey;
        ApiSecret = aesCrypt.Decrypt(secrets.TwitterApiSecretCrypt);
        AesCrypt = aesCrypt;

        Load(configMaster.Config.Value.Accounts);
    }
    private void Load(ImmutableArray<TwitterAccount> accounts)
    {
        foreach (var a in accounts)
        {
            var t = Tokens.Create(
                ApiKey,
                ApiSecret,
                AesCrypt.Decrypt(a.TokenCrypt),
                AesCrypt.Decrypt(a.TokenSecretCrypt),
                a.Id,
                a.ScreenName);
            UpdateTokens(t);
        }
    }
    private readonly Dictionary<long, TokensState> tokensDictionary = new();

    public string ApiKey { get; }
    public string ApiSecret { get; }
    public AesCrypt AesCrypt { get; }
    public TwitterAuthorizeSession AuthorizeSession() => new(this);

    public void AddTokens(Tokens tokens)
    {
        UpdateTokens(tokens, force: true);
    }
    private void UpdateTokens(Tokens tokens, bool force = false)
    {
        if (!force && tokensDictionary.TryGetValue(tokens.UserId, out var st)
#if NETFRAMEWORK
            && st.VerifyCredentialsTask.IsCompleted
#else
            && st.VerifyCredentialsTask.IsCompletedSuccessfully
#endif
        )
            return;

        tokensDictionary[tokens.UserId] = new(tokens);
    }
    public async ValueTask<Tokens?> GetTokensAsync(long id)
    {
        if (tokensDictionary.TryGetValue(id, out var state))
            return await state.GetVerifiedTokens().ConfigureAwait(false);
        return null;
    }

    public TwitterAccount TokenToAccount(Tokens tokens)
        => tokens.ToTwitterAccount(AesCrypt);

    private class TokensState
    {
        public TokensState(Tokens tokens)
        {
            Tokens = tokens;
            VerifyCredentialsTask = tokens.Account.VerifyCredentialsAsync();
        }
        internal Tokens Tokens { get; }
        internal Task<UserResponse> VerifyCredentialsTask { get; }
        public async ValueTask<Tokens> GetVerifiedTokens()
        {
            await VerifyCredentialsTask.ConfigureAwait(false);
            return Tokens;
        }
    }
}
