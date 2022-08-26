using CommunityToolkit.Mvvm.ComponentModel;
using CoreTweet;
using Kzrnm.TwitterJikkyo.Configs;
using Kzrnm.TwitterJikkyo.Twitter;
using System;
using System.Threading.Tasks;

namespace Kzrnm.TwitterJikkyo.Models;

public partial class AccountService : ObservableObject
{
    public AccountService(ConfigMaster configMaster, TwitterService twitterService)
    {
        ConfigMaster = configMaster;
        TwitterService = twitterService;

        LoadConfig(ConfigMaster.Config.Value);
        ConfigMaster.Config.ConfigUpdated += (_, e) => LoadConfig(e);
    }
    public ConfigMaster ConfigMaster { get; }
    public TwitterService TwitterService { get; }
    private bool CanChangeUser => ConfigMaster.Config.Value.Accounts.Length > 1;

    private int _PostingAccountIndex;
    private int _ImagePostingAccountIndex;

    [ObservableProperty]
    public Tokens? _PostingAccount;
    [ObservableProperty]
    public Tokens? _ImagePostingAccount;

    private async void LoadConfig(Config config)
    {
        var accounts = config.Accounts;
        for (int i = 0; i < accounts.Length; i++)
        {
            if (accounts[i].Id == config.DefaultAccountId)
                _PostingAccountIndex = i;
            if (accounts[i].Id == config.DefaultImageAccountId)
                _ImagePostingAccountIndex = i;
        }
        await UpdateTokensAsync().ConfigureAwait(false);
    }
    private async ValueTask UpdateTokensAsync()
    {
        var accounts = ConfigMaster.Config.Value.Accounts;

        if (accounts[_PostingAccountIndex].Id != PostingAccount?.UserId)
            PostingAccount = await TwitterService.GetTokensAsync(accounts[_PostingAccountIndex].Id).ConfigureAwait(false);
        if (accounts[_ImagePostingAccountIndex].Id != ImagePostingAccount?.UserId)
            ImagePostingAccount = await TwitterService.GetTokensAsync(accounts[_ImagePostingAccountIndex].Id).ConfigureAwait(false);
    }
    private static void UpdateIndex<T>(ReadOnlySpan<T> span, ref int index)
    {
        if ((uint)index < (uint)span.Length)
            return;
        else if (index >= span.Length)
            index = 0;
        else
            index = span.Length - 1;
    }
    public async ValueTask ActivatePreviousAccountAsync()
    {
        if (!CanChangeUser)
            return;

        --_PostingAccountIndex;
        UpdateIndex(ConfigMaster.Config.Value.Accounts.AsSpan(), ref _PostingAccountIndex);
        await UpdateTokensAsync().ConfigureAwait(false);
    }

    public async ValueTask ActivateNextAccountAsync()
    {
        if (!CanChangeUser)
            return;

        ++_PostingAccountIndex;
        UpdateIndex(ConfigMaster.Config.Value.Accounts.AsSpan(), ref _PostingAccountIndex);
        await UpdateTokensAsync().ConfigureAwait(false);
    }

    public async ValueTask ActivatePreviousImageAccountAsync()
    {
        if (!CanChangeUser)
            return;

        --_ImagePostingAccountIndex;
        UpdateIndex(ConfigMaster.Config.Value.Accounts.AsSpan(), ref _ImagePostingAccountIndex);
        await UpdateTokensAsync().ConfigureAwait(false);
    }

    public async ValueTask ActivateNextImageAccountAsync()
    {
        if (!CanChangeUser)
            return;

        ++_ImagePostingAccountIndex;
        UpdateIndex(ConfigMaster.Config.Value.Accounts.AsSpan(), ref _ImagePostingAccountIndex);
        await UpdateTokensAsync().ConfigureAwait(false);
    }
}
