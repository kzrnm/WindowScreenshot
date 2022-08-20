using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Kzrnm.WindowScreenshot.Image;
using System;

namespace Kzrnm.WindowScreenshot.ViewModels;

public partial class DebugAreaViewModel : ObservableObject
{
    public DebugAreaViewModel(ImageProvider imageProvider)
    {
        ImageProvider = imageProvider;
    }
    public ImageProvider ImageProvider { get; }

    [RelayCommand]
    private void AddRandomImage()
    {
        var pf = System.Windows.Media.PixelFormats.Rgb48;
        var width = Random.Shared.Next(1, 800);
        var height = Random.Shared.Next(1, 800);
        int stride = (width * pf.BitsPerPixel + 7) / 8; // 1行のビット数
        var bytes = new byte[stride * height];
        Random.Shared.NextBytes(bytes);
        var img = System.Windows.Media.Imaging.BitmapSource.Create(width, height, 96, 96, pf, null, bytes, stride);
        ImageProvider.AddImage(img);
    }
}
