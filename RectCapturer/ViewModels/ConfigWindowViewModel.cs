using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Kzrnm.RectCapturer.Configs;
using Kzrnm.Wpf.Font;
using Kzrnm.Wpf.Input;
using System.Diagnostics.CodeAnalysis;

namespace Kzrnm.RectCapturer.ViewModels;
public partial class ConfigWindowViewModel : ObservableObject
{
    public ConfigWindowViewModel(Config config, Shortcuts shortcuts)
    {
        Config = config;
        Shortcuts = shortcuts;
        Load(config, shortcuts);
        IsUpdated = false;
    }
    public Config Config { get; }
    public Shortcuts Shortcuts { get; }
    [MemberNotNull(nameof(_Font))]
    private void Load(Config config, Shortcuts shortcuts)
    {
        Topmost = config.Topmost;
        ShortcutPost = shortcuts.Post ?? default;
        ShortcutCaptureScreenshot = shortcuts.CaptureScreenshot ?? default;
        _Font = config.Font;
    }

    [ObservableProperty]
    private bool _IsUpdated;

    [ObservableProperty]
    private bool _Topmost;
    [SuppressMessage("Style", "IDE0060: Remove unused parameter")] partial void OnTopmostChanged(bool value) => IsUpdated = true;

    [ObservableProperty]
    private ShortcutKey _ShortcutCaptureScreenshot;
    [SuppressMessage("Style", "IDE0060: Remove unused parameter")] partial void OnShortcutCaptureScreenshotChanged(ShortcutKey value) => IsUpdated = true;

    [ObservableProperty]
    private ShortcutKey _ShortcutPost;
    [SuppressMessage("Style", "IDE0060: Remove unused parameter")] partial void OnShortcutPostChanged(ShortcutKey value) => IsUpdated = true;

    [ObservableProperty]
    private Font _Font;
    [SuppressMessage("Style", "IDE0060: Remove unused parameter")] partial void OnFontChanged(Font value) => IsUpdated = true;


    public (Config Config, Shortcuts Shortcuts) ToResult()
    {
        var config = Config with
        {
            Topmost = Topmost,
            Font = Font,
        };
        var shortcuts = Shortcuts with
        {
            CaptureScreenshot = ShortcutCaptureScreenshot,
            Post = ShortcutPost,
        };
        return (config, shortcuts);
    }

    [RelayCommand]
    private void UpdateFont()
    {
        var result = WeakReferenceMessenger.Default.Send(new FontDialogMessage(Font));
        if (result.Response is { } font)
        {
            Font = font;
        }
    }

    [RelayCommand]
    public void RestoreDefaultConfig()
    {
        Load(new(), new());
    }
}
