using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Kzrnm.TwitterJikkyo.Configs;
using Kzrnm.TwitterJikkyo.Models.Message;
using Kzrnm.TwitterJikkyo.Twitter;
using Kzrnm.Wpf.Font;
using Kzrnm.Wpf.Input;
using System;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace Kzrnm.TwitterJikkyo.ViewModels;
public partial class ConfigWindowViewModel : ObservableObject
{
    public ConfigWindowViewModel(TwitterService twitterService, Config config, Shortcuts shortcuts)
    {
        TwitterService = twitterService;
        Config = config;
        Shortcuts = shortcuts;
        Load(config, shortcuts);
        IsUpdated = false;
    }
    public TwitterService TwitterService { get; }
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
    private ShortcutKey _ActivatePreviousAccount;
    [SuppressMessage("Style", "IDE0060: Remove unused parameter")] partial void OnActivatePreviousAccountChanged(ShortcutKey value) => IsUpdated = true;
    [ObservableProperty]
    private ShortcutKey _ActivateNextAccount;
    [SuppressMessage("Style", "IDE0060: Remove unused parameter")] partial void OnActivateNextAccountChanged(ShortcutKey value) => IsUpdated = true;
    [ObservableProperty]
    private ShortcutKey _ActivatePreviousImageAccount;
    [SuppressMessage("Style", "IDE0060: Remove unused parameter")] partial void OnActivatePreviousImageAccountChanged(ShortcutKey value) => IsUpdated = true;
    [ObservableProperty]
    private ShortcutKey _ActivateNextImageAccount;
    [SuppressMessage("Style", "IDE0060: Remove unused parameter")] partial void OnActivateNextImageAccountChanged(ShortcutKey value) => IsUpdated = true;

    [ObservableProperty]
    private Font _Font;
    [SuppressMessage("Style", "IDE0060: Remove unused parameter")] partial void OnFontChanged(Font value) => IsUpdated = true;


    public ObservableCollection<TwitterAccount> Accounts { get; } = new();

    [ObservableProperty]
    private int _PostingAccountIndex;
    [ObservableProperty]
    private int _ImagePostingAccountIndex;


    [MemberNotNull(nameof(_Font))]
    private void Load(Config config, Shortcuts shortcuts)
    {
        Topmost = config.Topmost;
        ShortcutPost = shortcuts.Post ?? default;
        ShortcutCaptureScreenshot = shortcuts.CaptureScreenshot ?? default;
        ToggleHashtag = shortcuts.ToggleHashtag ?? default;
        InputInReplyTo = shortcuts.InputInReplyTo ?? default;
        ActivatePreviousAccount = shortcuts.ActivatePreviousAccount ?? default;
        ActivateNextAccount = shortcuts.ActivateNextAccount ?? default;
        ActivatePreviousImageAccount = shortcuts.ActivatePreviousImageAccount ?? default;
        ActivateNextImageAccount = shortcuts.ActivateNextImageAccount ?? default;

        Accounts.Clear();
        foreach (var a in config.Accounts.AsSpan())
            Accounts.Add(a);

        PostingAccountIndex = FindById(config.Accounts.AsSpan(), config.DefaultAccountId);
        ImagePostingAccountIndex = FindById(config.Accounts.AsSpan(), config.DefaultImageAccountId);

        _Font = config.Font;

        static int FindById(ReadOnlySpan<TwitterAccount> accounts, long id)
        {
            for (int i = 0; i < accounts.Length; i++)
            {
                if (accounts[i].Id == id)
                    return i;
            }
            return 0;
        }
    }
    public (Config Config, Shortcuts Shortcuts) ToResult()
    {
        var defaultAccount = (uint)PostingAccountIndex < (uint)Accounts.Count ? Accounts[PostingAccountIndex].Id : 0;
        var defaultImageAccount = (uint)ImagePostingAccountIndex < (uint)Accounts.Count ? Accounts[ImagePostingAccountIndex].Id : 0;

        var config = Config with
        {
            Topmost = Topmost,
            Font = Font,
            Accounts = Accounts.ToImmutableArray(),
            DefaultAccountId = defaultAccount,
            DefaultImageAccountId = defaultImageAccount,
        };
        var shortcuts = Shortcuts with
        {
            CaptureScreenshot = ShortcutCaptureScreenshot,
            Post = ShortcutPost,
            ToggleHashtag = ToggleHashtag,
            InputInReplyTo = InputInReplyTo,

            ActivatePreviousAccount = ActivatePreviousAccount,
            ActivateNextAccount = ActivateNextAccount,
            ActivatePreviousImageAccount = ActivatePreviousImageAccount,
            ActivateNextImageAccount = ActivateNextImageAccount,
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
    private async Task AddAccount()
    {
        var auth = TwitterService.AuthorizeSession();
        var result = WeakReferenceMessenger.Default.Send(new TwitetrAuthDialogMessage(auth.AuthorizeUri.ToString()));
        if (result.Response is { } pin)
        {
#pragma warning disable CAC002
            var tokens = await auth.AuthorizeAsync(pin).ConfigureAwait(true);
#pragma warning restore CAC002

            for (int i = Accounts.Count - 1; i >= 0; i--)
            {
                if (Accounts[i].Id == tokens.UserId)
                {
                    PostingAccountIndex = i;
                    Accounts[i] = TwitterService.TokenToAccount(tokens);
                    return;
                }
            }
            Accounts.Add(TwitterService.TokenToAccount(tokens));
            PostingAccountIndex = Accounts.Count - 1;
        }
    }
    [RelayCommand]
    private void RemoveAccount()
    {
        var index = PostingAccountIndex;
        var imageAccountIndex = ImagePostingAccountIndex;
        var imageAccountIndexDiff = imageAccountIndex - index;
        if ((uint)index < (uint)Accounts.Count)
        {
            Accounts.RemoveAt(index);
            if ((uint)index >= (uint)Accounts.Count)
                index = Accounts.Count - 1;
            PostingAccountIndex = index;

            if (imageAccountIndexDiff > 0)
                ImagePostingAccountIndex = imageAccountIndex;
            else if (imageAccountIndexDiff == 0)
                ImagePostingAccountIndex = index;
        }
    }
}
