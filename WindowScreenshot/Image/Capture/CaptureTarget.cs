using System;
using System.Text.Json.Serialization;
using System.Windows.Media.Imaging;

namespace Kzrnm.WindowScreenshot.Image.Capture;

/// <summary>
/// キャプチャ対象
/// </summary>
/// <param name="ProcessName">対象のプロセス名</param>
/// <param name="WindowName">対象のウインドウ名</param>
/// <param name="Region">キャプチャ領域</param>
/// <param name="OnlyTargetWindow">true なら対象のウインドウのみを対象にする。false なら <see cref="Windows.Win32.PInvoke.ClientToScreen"/>でスクリーンに表示されているものを対象にする</param>
public record CaptureTarget(
    string ProcessName,
    string WindowName,
    CaptureRegion Region,
    bool OnlyTargetWindow)
{
    public bool IsFitFor(WindowProcessHandle window) => IsFitFor(window, ProcessName, WindowName);
    public static bool IsFitFor(WindowProcessHandle window, string? targetProcessName, string? targetWindowName)
    {
        if (!window.IsActive)
            return false;

        if (targetProcessName != null
            && (window.ProcessName.IndexOf(targetProcessName, StringComparison.OrdinalIgnoreCase) < 0))
        {
            return false;
        }
        if (targetWindowName != null
            && (window.GetCurrentWindowName().IndexOf(targetWindowName, StringComparison.OrdinalIgnoreCase) < 0))
        {
            return false;
        }
        return true;
    }

    public BitmapSource? CaptureFrom(WindowProcessHandle[] windowProcessHandles)
    {
        foreach (var window in windowProcessHandles)
        {
            if (IsFitFor(window))
            {
                return window.GetClientBitmap(Region, OnlyTargetWindow);
            }
        }
        return default;
    }
}