using CommunityToolkit.Mvvm.ComponentModel;
using Kzrnm.WindowScreenshot.Image;
using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace Kzrnm.TwitterJikkyo.Models;

public partial class ContentService : ObservableObject
{
    public ContentService(ImageProvider imageProvider)
    {
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

    private void UpdateCanPost()
    {
        CanPost = true;
    }

    public string TweetText => $"{Hashtag}";

    public async Task PostContentAsync()
    {
        await Task.Yield();
        System.Diagnostics.Debug.WriteLine("ツイートする Text:{0} Hashtag:{1}", Text, Hashtag);
    }
}
