using CommunityToolkit.Mvvm.DependencyInjection;
using Kzrnm.WindowScreenshot.Configs;
using System;
using System.Windows;

namespace Kzrnm.WindowScreenshot.Views;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        ConfigMaster = Ioc.Default.GetService<ConfigMaster>();
        if (ConfigMaster?.Config is { } confWrapper)
        {
            confWrapper.Value.WindowPosition.ApplyTo(this);
            Topmost = confWrapper.Value.Topmost;
            confWrapper.ConfigUpdated += (sender, e) =>
            {
                Topmost = e.Config.Topmost;
            };
        }
    }

    private ConfigMaster? ConfigMaster { get; }

    protected override async void OnClosed(EventArgs e)
    {
        base.OnClosed(e);
        if (ConfigMaster is { } configMaster)
        {
            configMaster.Config.Value = configMaster.Config.Value with { WindowPosition = WindowPosition.Load(this) };
            await configMaster.SaveAsync().ConfigureAwait(false);
        }
    }
}
