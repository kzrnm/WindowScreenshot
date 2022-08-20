﻿using Kzrnm.WindowScreenshot.Configs;
using Kzrnm.Wpf.Configs;
using System;
using System.Configuration;
using System.Threading.Tasks;

namespace Kzrnm.WindowScreenshot;

public partial class ConfigMaster
{
    public ConfigMaster(ConfigWrapper<Config> config, ConfigWrapper<CaptureWindowCollection> captureWindows, ConfigWrapper<Shortcuts> shortcuts)
    {
        Config = config;
        CaptureWindows = captureWindows;
        Shortcuts = shortcuts;
    }

    public ConfigWrapper<Config> Config { get; }
    public ConfigWrapper<CaptureWindowCollection> CaptureWindows { get; }
    public ConfigWrapper<Shortcuts> Shortcuts { get; }

    public static async Task<ConfigMaster> LoadConfigsAsync()
    {
        var config = ConfigWrapper<Config>.LoadAsync(ConfigurationManager.AppSettings["Config"] ?? throw new NullReferenceException("AppSettings:Config")).ConfigureAwait(false);
        var shortcuts = ConfigWrapper<Shortcuts>.LoadAsync(ConfigurationManager.AppSettings["Shortcuts"] ?? throw new NullReferenceException("AppSettings:Shortcuts")).ConfigureAwait(false);
        var captureWindows = ConfigWrapper<CaptureWindowCollection>.LoadAsync(ConfigurationManager.AppSettings["CaptureWindowCollection"] ?? throw new NullReferenceException("AppSettings:CaptureWindowCollection")).ConfigureAwait(false);

        return new(await config, await captureWindows, await shortcuts);
    }

    public Task SaveAsync() => Task.WhenAll(new[]
    {
        Config.SaveAsync(),
        CaptureWindows.SaveAsync(),
        Shortcuts.SaveAsync(),
    });
}