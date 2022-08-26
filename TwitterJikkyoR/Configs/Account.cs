namespace Kzrnm.TwitterJikkyo.Configs;

public record Account(long Id = 0, string ScreenName = "", string TokenCrypt = "", string TokenSecretCrypt = "")
{
    public Account() : this(ScreenName: "") { }
}
