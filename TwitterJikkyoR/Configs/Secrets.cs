namespace Kzrnm.TwitterJikkyo.Configs;
public record Secrets(byte[] AesKey = null!, byte[] AesIv = null!, string TwitterApiKey = "", byte[] TwitterApiSecretCrypt = null!)
{
    public Secrets() : this(TwitterApiKey: "") { }
}
