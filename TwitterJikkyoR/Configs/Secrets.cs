﻿namespace Kzrnm.TwitterJikkyo.Configs;
public record Secrets(byte[] AesKey = null!, byte[] AesIv = null!, string TwitterApiToken = "", string TwitterApiSecret = "")
{
    public Secrets() : this(TwitterApiSecret: "") { }
}