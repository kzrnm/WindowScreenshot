using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using CoreTweet;
using Kzrnm.TwitterJikkyo.Models.Message;
using Kzrnm.TwitterJikkyo.Properties;
using Kzrnm.TwitterJikkyo.Twitter;
using Kzrnm.WindowScreenshot.Image;
using Kzrnm.WindowScreenshot.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace Kzrnm.TwitterJikkyo.Models;

public partial class ContentService : ObservableObject
{
    public ContentService(AccountService accountService, ImageProvider imageProvider)
    {
        AccountService = accountService;
        ImageProvider = imageProvider;
        UpdateCanPost();
        ((INotifyPropertyChanged)ImageProvider.Images).PropertyChanged += OnImageProviderPropertyChanged;
    }

    private void OnImageProviderPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case nameof(ImageProvider.Images.Count):
                UpdateCanPost();
                return;
        }
    }

    public event EventHandler<bool>? CanPostChanged;
    public AccountService AccountService { get; }
    public ImageProvider ImageProvider { get; }

    private bool _CanPost;
    public bool CanPost
    {
        private set
        {
            if (SetProperty(ref _CanPost, value))
            {
                CanPostChanged?.Invoke(this, value);
            }
        }
        get => _CanPost;
    }

    [ObservableProperty]
    private string _Hashtag = "";
    [SuppressMessage("Style", "IDE0060: Remove unused parameter")]
    partial void OnHashtagChanged(string value)
    {
        UpdateCanPost();
    }

    [ObservableProperty]
    private string _Text = "";
    [SuppressMessage("Style", "IDE0060: Remove unused parameter")]
    partial void OnTextChanged(string value)
    {
        UpdateCanPost();
    }

    [ObservableProperty]
    private string _InReplyToText = "";

    private void UpdateCanPost()
    {
        RemainingTextLength = TweetTextStringInfo.GetRemainingTweetTextLength(TweetText);
        CanPost = RemainingTextLength >= 0 && (Text.Length > 0 || ImageProvider.Images.Count > 0);
    }

    [ObservableProperty]
    private int _RemainingTextLength;

    public string TweetText => (Text, Hashtag) switch
    {
        (var text, "") => text,
        (var text, var hashtag) => $"{text} #{hashtag}",
    };
    internal Tokens? SelectTokens()
    {
        if (ImageProvider.Images.Count > 0 && AccountService.ImagePostingAccount is { } imageTokens)
            return imageTokens;
        return AccountService.PostingAccount;
    }

    public async Task<StatusResponse?> PostContentAsync()
    {
        var tokens = SelectTokens();

        if (tokens is null)
        {
            WeakReferenceMessenger.Default.Send(new DialogMessage
            {
                Caption = Resources.MainWindowTitle,
                Text = Resources.NotLoggedIn,
                MessageBoxButton = MessageBoxButton.OK,
                MessageBoxImage = MessageBoxImage.Error,
            });
            return null;
        }
        _ = TryParseTweetId(InReplyToText, out var inReplyToId);
        var tweetText = TweetText;
        var text = Text;
        var hashtag = Hashtag;
        var inReplyToText = InReplyToText;
        var images = ImageProvider.Images.ToArray();
        Text = "";
        Hashtag = "";
        InReplyToText = "";
        ImageProvider.Images.Clear();

        try
        {
#pragma warning disable CAC002 // ConfigureAwaitChecker
            var response = await PostContentAsync(tokens, tweetText, inReplyToId, images).ConfigureAwait(true);
#pragma warning restore CAC002 // ConfigureAwaitChecker
            return response;
        }
        catch (TwitterException)
        {
            if (WeakReferenceMessenger.Default.Send(new DialogMessage
            {
                Caption = Resources.FailedToPost,
                Text = Resources.FailedToPostMessage,
                MessageBoxButton = MessageBoxButton.OKCancel,
                MessageBoxImage = MessageBoxImage.Exclamation,
            }).Response is MessageBoxResult.OK)
            {
                // Restore
                Text = text;
                Hashtag = hashtag;
                InReplyToText = inReplyToText;
                foreach (var img in images)
                    ImageProvider.Images.Add(img);
            }
        }
        return null;
    }

    private async Task<StatusResponse> PostContentAsync(Tokens tokens, string text, long inReplyToId, CaptureImage[] images)
    {
        IEnumerable<long>? mediaIds = null;
        if (images.Length > 0)
        {
            var imgUploadTasks = images
                .Select(img => img.ToStream())
                .Select(s => tokens.Media.UploadAsync(s));
            mediaIds = (await Task.WhenAll(imgUploadTasks).ConfigureAwait(false)).Select(t => t.MediaId);
        }

        return await tokens.Statuses.UpdateAsync(
            status: text,
            in_reply_to_status_id: inReplyToId,
            auto_populate_reply_metadata: true,
            media_ids: mediaIds
        ).ConfigureAwait(false);
    }

    internal static bool TryParseTweetId(string idOrUrl, out long tweetId)
    {
        tweetId = default;
        if (string.IsNullOrEmpty(idOrUrl))
            return false;

        if (long.TryParse(idOrUrl, out tweetId))
            return true;

        var m = inReplyToRegex.Match(idOrUrl);
        return m.Success && long.TryParse(m.Groups[1].Value, out tweetId);
    }
    static Regex inReplyToRegex = new(@"twitter.com/[^/]+/status/(\d+)", RegexOptions.Compiled);

    public void TryInputInReplyTo()
    {
        if (WeakReferenceMessenger.Default.Send(new InReplyToDialogMessage(InReplyToText)).Response is { } res)
            InReplyToText = res;
    }
}
