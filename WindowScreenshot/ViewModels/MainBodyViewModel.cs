using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Kzrnm.WindowScreenshot.Image;
using System.Diagnostics;

namespace Kzrnm.WindowScreenshot.ViewModels;

public partial class MainBodyViewModel : ObservableObject
{
    public MainBodyViewModel(ImageProvider imageProvider)
    {
        ImageProvider = imageProvider;
    }
    public ImageProvider ImageProvider { get; }

    [RelayCommand]
    private void OpenSelectCaptureWindowDialog()
    {
        // TODO: OpenSelectCaptureWindowDialog
        Debug.WriteLine("OpenSelectCaptureWindowDialogCommand");
    }
    [RelayCommand]
    private void OpenConfigDialog()
    {
        // TODO: OpenConfigDialog
        Debug.WriteLine("OpenConfigDialogCommand");
    }
    [RelayCommand]
    private void CaptureScreenshot()
    {
        // TODO: CaptureScreenshot
        Debug.WriteLine("CaptureScreenshotCommand");
    }
    [RelayCommand]
    private void PostContent()
    {
        // TODO: PostContent
        Debug.WriteLine("PostContentCommand");
    }

}
