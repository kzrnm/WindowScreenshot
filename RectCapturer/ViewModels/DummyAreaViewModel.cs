using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Kzrnm.RectCapturer.Models;
using Kzrnm.WindowScreenshot.Models;
using System;

namespace Kzrnm.RectCapturer.ViewModels;

public partial class DebugAreaViewModel : ObservableObject
{
    public DebugAreaViewModel(GlobalService globalService)
    {
        GlobalService = globalService;
    }
    public GlobalService GlobalService { get; }

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
        GlobalService.ImageProvider.AddImage(new(img));
    }

    [RelayCommand]
    private void PasteImage()
        => GlobalService.PasteImageFromClipboard();
}
