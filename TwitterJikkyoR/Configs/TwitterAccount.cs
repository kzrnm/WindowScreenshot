using CoreTweet;
using Kzrnm.TwitterJikkyo.Logic;
using System;

namespace Kzrnm.TwitterJikkyo.Configs;

public record TwitterAccount(long Id = 0, string ScreenName = "", byte[]? TokenCrypt = null, byte[]? TokenSecretCrypt = null)
{
    public byte[] TokenCrypt { get; init; } = TokenCrypt ?? Array.Empty<byte>();
    public byte[] TokenSecretCrypt { get; init; } = TokenSecretCrypt ?? Array.Empty<byte>();
    public TwitterAccount() : this(ScreenName: "") { }
}
public static class TokensToTwitterAccountExtension
{
    public static TwitterAccount ToTwitterAccount(this Tokens tokens, AesCrypt aes)
    {
        return new(tokens.UserId, tokens.ScreenName, aes.Encrypt(tokens.AccessToken), aes.Encrypt(tokens.AccessTokenSecret));
    }
}