using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Kzrnm.WindowScreenshot.Image;
using System;

namespace Kzrnm.WindowScreenshot.ViewModels;

public partial class MainBodyViewModel : ObservableObject
{
    public MainBodyViewModel(ImageProvider imageProvider)
    {
        ImageProvider = imageProvider;
    }
    public ImageProvider ImageProvider { get; }

    [RelayCommand]
    private void PostContent()
    {
        // TODO: PostContent
    }
}
