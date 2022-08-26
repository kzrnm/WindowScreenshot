using CommunityToolkit.Mvvm.DependencyInjection;
using Kzrnm.TwitterJikkyo.Configs;
using Kzrnm.TwitterJikkyo.Logic;
using Kzrnm.TwitterJikkyo.Models;
using Kzrnm.TwitterJikkyo.Twitter;
using Kzrnm.TwitterJikkyo.ViewModels;
using Kzrnm.WindowScreenshot;
using Kzrnm.Wpf.Configs;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows;

namespace Kzrnm.TwitterJikkyo;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public static string ExePath => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? throw new InvalidOperationException();
    public App()
    {
        var secrets = ConfigWrapper<Secrets>.LoadAsync(ConfigurationManager.AppSettings["Secrets"] ?? throw new NullReferenceException("AppSettings:Secrets")).Result.Value;

        Ioc.Default.ConfigureServices(
            new ServiceCollection()
            .InitializeWindowScreenshot()
            .AddSingleton(secrets)
            .AddSingleton(new AesCrypt(secrets.AesKey, secrets.AesIv))
            .AddSingleton(ConfigMaster.LoadConfigsAsync(ConfigurationManager.AppSettings).Result)
            .AddSingleton<TwitterService>()
            .AddSingleton<GlobalService>()
            .AddSingleton<ContentService>()
            .AddSingleton<AccountService>()
            .AddTransient<MainWindowViewModel>()
            .AddTransient<MainBodyViewModel>()
            .BuildServiceProvider()
            );
#if !DEBUG
        AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
#endif
    }

    private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        if (e.ExceptionObject is not Exception exception) return;

        Debug.WriteLine(exception);
        Debug.WriteLine(exception.StackTrace);
        var logdir = Path.Combine(ExePath, "Log");
        Directory.CreateDirectory(logdir);

        var filePath = Path.Combine(logdir, $"{Assembly.GetExecutingAssembly().GetName().Name}-{DateTime.Now:yyyyMMddHHmmss}.log");
        using var fw = new StreamWriter(filePath, true, new UTF8Encoding(false));
        using var sr = new StringReader(exception.StackTrace ?? "");

        fw.WriteLine("Message： " + exception.Message);
        while (sr.ReadLine() is string line)
            fw.WriteLine(line);
        fw.Flush();

        MessageBox.Show("Error", "UnhandledException", MessageBoxButton.OK, MessageBoxImage.Stop);
        Environment.Exit(1);
    }
}
