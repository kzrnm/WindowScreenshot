﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Kzrnm.TwitterJikkyo.Configs;
using Kzrnm.TwitterJikkyo.Models;
using Kzrnm.TwitterJikkyo.Models.Message;
using Kzrnm.WindowScreenshot;
using Kzrnm.WindowScreenshot.Image;
using Kzrnm.WindowScreenshot.Image.DragDrop;
using Kzrnm.WindowScreenshot.Models;
using System.Collections.Specialized;
using System.Threading.Tasks;

namespace Kzrnm.TwitterJikkyo.ViewModels;

public partial class MainBodyViewModel : ObservableObject
{
    public MainBodyViewModel(GlobalService globalService, ContentService contentService, ImageDropTarget.Factory imageDropTargetFactory)
    {
        GlobalService = globalService;
        ContentService = contentService;
        ImageProvider = globalService.ImageProvider;
        ConfigMaster = globalService.ConfigMaster;
        DropHandler = imageDropTargetFactory.Build(true);

        Hashtags = ConfigMaster.Hashtags.Value;
        Hashtags.SelectedIndex = -1;
        Hashtags.CollectionChanged += OnHashtagsCollectionChanged;

        contentService.CanPostChanged += (_, _) => postContentCommand?.NotifyCanExecuteChanged();
    }

    private async void OnHashtagsCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        await ConfigMaster.Hashtags.SaveAsync().ConfigureAwait(false);
    }

    public ContentService ContentService { get; }
    public ImageDropTarget DropHandler { get; }
    public GlobalService GlobalService { get; }
    public ImageProvider ImageProvider { get; }
    public ConfigMaster ConfigMaster { get; }
    public SelectorObservableCollection<string> Hashtags { get; }

    [ObservableProperty]
    private int _RemainingTextLength;

    [RelayCommand]
    private void OpenSelectCaptureWindowDialog()
    {
        var result = WeakReferenceMessenger.Default.Send(new CaptureTargetSelectionDialogMessage(ConfigMaster.CaptureTargetWindows.Value));
        if (result.Response is { } collection)
        {
            ConfigMaster.CaptureTargetWindows.Value = new CaptureWindowCollection(collection);
        }
    }
    [RelayCommand]
    private async Task OpenConfigDialog()
    {
        var result = WeakReferenceMessenger.Default.Send(new ConfigDialogMessage(ConfigMaster.Config.Value, ConfigMaster.Shortcuts.Value));
        if (result.Response is var (config, shortcuts) && config is not null && shortcuts is not null)
        {
            ConfigMaster.Config.Value = config;
            ConfigMaster.Shortcuts.Value = shortcuts;
            await ConfigMaster.SaveAsync().ConfigureAwait(false);
        }
    }
    [RelayCommand]
    private void CaptureScreenshot()
        => GlobalService.CaptureScreenshot();

    private bool CanPostContent() => ContentService.CanPost;
    [RelayCommand(CanExecute = nameof(CanPostContent))]
    private async Task PostContent()
        => await ContentService.PostContentAsync().ConfigureAwait(false);

    [RelayCommand]
    private void InputInReplyTo()
    {
        ContentService.TryInputInReplyTo();
    }
}
