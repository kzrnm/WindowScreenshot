using Kzrnm.TwitterJikkyo.Configs;
using Kzrnm.Wpf.Configs;
using System;
using System.Configuration;
using System.Threading.Tasks;

namespace Kzrnm.TwitterJikkyo;

public partial class ConfigMaster
{
    public ConfigMaster(ConfigWrapper<Config> config, ConfigWrapper<CaptureWindowCollection> captureWindows, ConfigWrapper<Shortcuts> shortcuts, ConfigWrapper<Hashtags> hashtags)
    {
        Config = config;
        CaptureTargetWindows = captureWindows;
        Shortcuts = shortcuts;
        Hashtags = hashtags;
    }

    public ConfigWrapper<Config> Config { get; }
    public ConfigWrapper<CaptureWindowCollection> CaptureTargetWindows { get; }
    public ConfigWrapper<Shortcuts> Shortcuts { get; }
    public ConfigWrapper<Hashtags> Hashtags { get; }

    public static async Task<ConfigMaster> LoadConfigsAsync()
    {
        var config = ConfigWrapper<Config>.LoadAsync(ConfigurationManager.AppSettings["Config"] ?? throw new NullReferenceException("AppSettings:Config")).ConfigureAwait(false);
        var shortcuts = ConfigWrapper<Shortcuts>.LoadAsync(ConfigurationManager.AppSettings["Shortcuts"] ?? throw new NullReferenceException("AppSettings:Shortcuts")).ConfigureAwait(false);
        var captureWindows = ConfigWrapper<CaptureWindowCollection>.LoadAsync(ConfigurationManager.AppSettings["CaptureWindowCollection"] ?? throw new NullReferenceException("AppSettings:CaptureWindowCollection")).ConfigureAwait(false);
        var hashtags = ConfigWrapper<Hashtags>.LoadAsync(ConfigurationManager.AppSettings["Hashtags"] ?? throw new NullReferenceException("AppSettings:Hashtags")).ConfigureAwait(false);
        return new(await config, await captureWindows, await shortcuts, await hashtags);
    }

    public Task SaveAsync() => Task.WhenAll(new[]
    {
        Config.SaveAsync(),
        CaptureTargetWindows.SaveAsync(),
        Shortcuts.SaveAsync(),
        Hashtags.SaveAsync(),
    });
}