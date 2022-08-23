﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Kzrnm.RectCapturer.Properties;
using Kzrnm.WindowScreenshot.Image;
using Kzrnm.WindowScreenshot.Image.DragDrop;
using System.Windows.Input;

namespace Kzrnm.RectCapturer.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    public MainWindowViewModel(ImageProvider imageProvider, ImageDropTarget.Factory imageDropTargetFactory)
    {
        ImageProvider = imageProvider;
        DropHandler = imageDropTargetFactory.Build(true);
    }
    public ImageProvider ImageProvider { get; }
    public ImageDropTarget DropHandler { get; }
    public string Title { get; } = Resources.MainWindowTitle;

    [RelayCommand]
    private void ClearImage() => ImageProvider.Images.Clear();
    private void OnKeyDown(KeyEventArgs e)
    {
        switch (e.Key)
        {
            case Key.Escape:
                ImageProvider.Images.Clear();
                break;
        }
    }
}