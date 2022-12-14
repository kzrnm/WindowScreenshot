using CommunityToolkit.Mvvm.DependencyInjection;
using Kzrnm.TwitterJikkyo.Configs;
using Kzrnm.TwitterJikkyo.Logic;
using Kzrnm.TwitterJikkyo.Models;
using Kzrnm.TwitterJikkyo.Twitter;
using Kzrnm.TwitterJikkyo.ViewModels;
using Kzrnm.TwitterJikkyo.Views;
using Kzrnm.WindowScreenshot;
using Kzrnm.Wpf.Configs;
using Kzrnm.Wpf.Utility;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;

namespace Kzrnm.TwitterJikkyo;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public static string ExePath => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? throw new InvalidOperationException();
    public App()
    {
        DispatcherUnhandledException += OnDispatcherUnhandledException;
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
#pragma warning disable CAC002 // ConfigureAwaitChecker
        var secretsWrapper = await ConfigWrapper<Secrets>.LoadAsync(
            ConfigurationManager.AppSettings["Secrets"] ?? throw new NullReferenceException("AppSettings:Secrets")
        ).ConfigureAwait(true);
        var configMaster = await ConfigMaster.LoadConfigsAsync(ConfigurationManager.AppSettings).ConfigureAwait(true);
#pragma warning restore CAC002 // ConfigureAwaitChecker

        var secrets = secretsWrapper.Value;
        Ioc.Default.ConfigureServices(
            new ServiceCollection()
            .InitializeWindowScreenshot(imageProvider: new TweetImageProvider())
            .AddSingleton<ITwitterTokenService, TwitterTokenService>()
            .AddSingleton<ITwitterApiService, TwitterApiService>()
            .AddSingleton(secrets)
            .AddSingleton(new AesCrypt(secrets.AesKey, secrets.AesIv))
            .AddSingleton(configMaster)
            .AddSingleton<GlobalService>()
            .AddSingleton<ContentService>()
            .AddSingleton<AccountService>()
            .AddSingleton<ConfigWindowViewModel.Factory>()
            .AddTransient<MainWindowViewModel>()
            .AddTransient<MainBodyViewModel>()
            .BuildServiceProvider()
        );
        new MainWindow().Show();
        base.OnStartup(e);
    }

    private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
#if DEBUG
        AppUtility.OnUnhandledException_Debug(sender, e);
#else
        AppUtility.OnUnhandledException_Release(sender, e);
#endif
    }
}
