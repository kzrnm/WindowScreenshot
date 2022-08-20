using CommunityToolkit.Mvvm.ComponentModel;
using Kzrnm.WindowScreenshot.Configs;
using Kzrnm.WindowScreenshot.Properties.App;
using Kzrnm.Wpf.Configs;

namespace Kzrnm.WindowScreenshot.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    public MainWindowViewModel(ConfigMaster configMaster)
    {
        ConfigMaster = configMaster;
        configMaster.Config.ConfigUpdated += OnConfigUpdated;
    }
    public ConfigMaster ConfigMaster { get; }
    public string Title { get; } = Resources.MainWindowTitle;

    private void OnConfigUpdated(object? sender, ConfigUpdatedEventArgs<Config> e)
    {
        var config = e.Config;
        Topmost = config.Topmost;
    }


    [ObservableProperty]
    private bool _Topmost;
}
