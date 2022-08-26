using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Kzrnm.TwitterJikkyo.Configs;
using Kzrnm.Wpf.Font;
using Kzrnm.Wpf.Input;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;

namespace Kzrnm.TwitterJikkyo.ViewModels;
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
    private ShortcutKey _ToggleHashtag;
    [SuppressMessage("Style", "IDE0060: Remove unused parameter")] partial void OnToggleHashtagChanged(ShortcutKey value) => IsUpdated = true;

    [ObservableProperty]
    private ShortcutKey _InputInReplyTo;
    [SuppressMessage("Style", "IDE0060: Remove unused parameter")] partial void OnInputInReplyToChanged(ShortcutKey value) => IsUpdated = true;

    [ObservableProperty]
    private ShortcutKey _ActivatePreviousUser;
    [SuppressMessage("Style", "IDE0060: Remove unused parameter")] partial void OnActivatePreviousUserChanged(ShortcutKey value) => IsUpdated = true;
    [ObservableProperty]
    private ShortcutKey _ActivateNextUser;
    [SuppressMessage("Style", "IDE0060: Remove unused parameter")] partial void OnActivateNextUserChanged(ShortcutKey value) => IsUpdated = true;
    [ObservableProperty]
    private ShortcutKey _ActivatePreviousImageUser;
    [SuppressMessage("Style", "IDE0060: Remove unused parameter")] partial void OnActivatePreviousImageUserChanged(ShortcutKey value) => IsUpdated = true;
    [ObservableProperty]
    private ShortcutKey _ActivateNextImageUser;
    [SuppressMessage("Style", "IDE0060: Remove unused parameter")] partial void OnActivateNextImageUserChanged(ShortcutKey value) => IsUpdated = true;

    [ObservableProperty]
    private Font _Font;
    [SuppressMessage("Style", "IDE0060: Remove unused parameter")] partial void OnFontChanged(Font value) => IsUpdated = true;


    public ObservableCollection<Account> Accounts { get; } = new();

    [ObservableProperty]
    private int _AccountForPostIndex;
    [ObservableProperty]
    private int _AccountForPostImageIndex;


    [MemberNotNull(nameof(_Font))]
    private void Load(Config config, Shortcuts shortcuts)
    {
        Topmost = config.Topmost;
        ShortcutPost = shortcuts.Post ?? default;
        ShortcutCaptureScreenshot = shortcuts.CaptureScreenshot ?? default;
        ToggleHashtag = shortcuts.ToggleHashtag ?? default;
        InputInReplyTo = shortcuts.InputInReplyTo ?? default;
        ActivatePreviousUser = shortcuts.ActivatePreviousUser ?? default;
        ActivateNextUser = shortcuts.ActivateNextUser ?? default;
        ActivatePreviousImageUser = shortcuts.ActivatePreviousImageUser ?? default;
        ActivateNextImageUser = shortcuts.ActivateNextImageUser ?? default;

        Accounts.Clear();
        foreach (var a in config.Accounts.AsSpan())
            Accounts.Add(a);
        _Font = config.Font;
    }
    public (Config Config, Shortcuts Shortcuts) ToResult()
    {
        var config = Config with
        {
            Topmost = Topmost,
            Font = Font,
            Accounts = Accounts.ToImmutableArray(),
        };
        var shortcuts = Shortcuts with
        {
            CaptureScreenshot = ShortcutCaptureScreenshot,
            Post = ShortcutPost,
            ToggleHashtag = ToggleHashtag,
            InputInReplyTo = InputInReplyTo,

            ActivatePreviousUser = ActivatePreviousUser,
            ActivateNextUser = ActivateNextUser,
            ActivatePreviousImageUser = ActivatePreviousImageUser,
            ActivateNextImageUser = ActivateNextImageUser,
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


    [RelayCommand]
    private void AddAccount()
    {
    }
    [RelayCommand]
    private void RemoveAccount()
    {
        var index = AccountForPostIndex;
        var imageAccountIndex = AccountForPostImageIndex;
        var imageAccountIndexDiff = imageAccountIndex - index;
        if ((uint)index < (uint)Accounts.Count)
        {
            Accounts.RemoveAt(index);
            if ((uint)index >= (uint)Accounts.Count)
                index = Accounts.Count - 1;
            AccountForPostIndex = index;

            if (imageAccountIndexDiff > 0)
                AccountForPostImageIndex = imageAccountIndex;
            else if (imageAccountIndexDiff == 0)
                AccountForPostImageIndex = index;
        }
    }
}
