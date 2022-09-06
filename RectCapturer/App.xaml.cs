using CommunityToolkit.Mvvm.DependencyInjection;
using Kzrnm.RectCapturer.Models;
using Kzrnm.RectCapturer.ViewModels;
using Kzrnm.RectCapturer.Views;
using Kzrnm.WindowScreenshot;
using Kzrnm.Wpf.Utility;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Windows;
using System.Windows.Threading;

namespace Kzrnm.RectCapturer;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public App()
    {
        DispatcherUnhandledException += OnDispatcherUnhandledException;
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
#pragma warning disable CAC002 // ConfigureAwaitChecker
        var configMaster = await ConfigMaster.LoadConfigsAsync(ConfigurationManager.AppSettings).ConfigureAwait(true);
#pragma warning restore CAC002 // ConfigureAwaitChecker

        Ioc.Default.ConfigureServices(
            new ServiceCollection()
            .InitializeWindowScreenshot()
            .AddSingleton(configMaster)
            .AddSingleton<GlobalService>()
            .AddSingleton<ContentService>()
            .AddTransient<MainWindowViewModel>()
            .AddTransient<MainBodyViewModel>()
#if DEBUG
            .AddTransient<DebugAreaViewModel>()
#endif
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
