using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Kzrnm.WindowScreenshot.Image;
using Kzrnm.WindowScreenshot.Models;
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
        WeakReferenceMessenger.Default.Send(new CaptureTargetSelectionDialogMessage());
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
