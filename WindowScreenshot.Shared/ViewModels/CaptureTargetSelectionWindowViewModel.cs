using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Kzrnm.WindowScreenshot.Image.Capture;
using System;

namespace Kzrnm.WindowScreenshot.ViewModels;
public partial class CaptureTargetSelectionWindowViewModel : ObservableObject
{
    public CaptureTargetSelectionWindowViewModel()
    {
    }
    public SelectorObservableCollection<CaptureTarget> CaptureTargetWindows { get; } = new();

    [ObservableProperty]
    private CaptureTarget? _SelectedTarget;


    [RelayCommand]
    public void RemoveSelectedCaptureWindow()
    {
        CaptureTargetWindows.RemoveSelectedItem();
    }
}
