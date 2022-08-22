using CommunityToolkit.Mvvm.ComponentModel;
using System;
using Windows.Win32.Foundation;

namespace Kzrnm.WindowScreenshot.Image.Capture;

public partial class CaptureRegion : ObservableObject, ICloneable
{
    [ObservableProperty]
    private int _Left;

    [ObservableProperty]
    private int _Right;

    [ObservableProperty]
    private int _Top;

    [ObservableProperty]
    private int _Bottom;

    [ObservableProperty]
    private int _Width;

    [ObservableProperty]
    private int _Height;

    [ObservableProperty]
    private bool _UseRect;

    public (int pointX, int pointY, int width, int height) GetTargetRegion(in RECT windowRect)
    {
        int left = windowRect.left + Left;
        int top = windowRect.top + Top;

        if (UseRect)
        {
            return (left, top, Width, Height);
        }
        else
        {
            return (left, top,
                windowRect.right - Right - left,
                windowRect.bottom - Bottom - top);
        }
    }

    object ICloneable.Clone() => Clone();
    public CaptureRegion Clone() => (CaptureRegion)MemberwiseClone();

}
