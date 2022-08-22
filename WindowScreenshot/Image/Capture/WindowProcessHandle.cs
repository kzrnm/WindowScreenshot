using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using Windows.Win32.Foundation;
using Windows.Win32.Graphics.Gdi;
using static Kzrnm.WindowScreenshot.Windows.NativeMethods;
using static Windows.Win32.PInvoke;

namespace Kzrnm.WindowScreenshot.Image.Capture;
public class WindowProcessHandle : IDisposable
{
    public HWND Handle { get; private set; }
    public string GetCurrentWindowName() => GetWindowText(Handle);

    public string ProcessName { get; }
    public string DefaultWindowName { get; }

    private bool disposed = false;

    public WindowProcessHandle(nint handle) : this((HWND)handle) { }
    internal WindowProcessHandle(HWND handle)
    {
        Handle = handle;
        _ = GetWindowThreadProcessId(handle, out var processId);
        using (var process = Process.GetProcessById((int)processId))
            ProcessName = process.ProcessName;
        DefaultWindowName = GetCurrentWindowName();
    }

    public bool IsActive => IsWindow(Handle);
    public override string ToString() => GetCurrentWindowName();

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposed) return;
        if (disposing)
        {
            Handle = default;
            disposed = true;
        }
    }

    public BitmapSource? GetClientBitmap(CaptureRegion region) => this.GetClientBitmap(region, false);
    public BitmapSource? GetClientBitmap(CaptureRegion region, bool onlyTargetWindow)
    {
        if (GetClientRect(Handle, out var rect) == 0) return null;

        var (left, top, width, height) = region.GetTargetRegion(rect);

        if (width <= 0 || height <= 0) return null;

        return Capture(left, top, width, height, onlyTargetWindow);
    }

    private BitmapSource Capture(int pointX, int pointY, int width, int height, bool onlyTargetWindow)
    {
        var hdc = Handle;

        var pt = new POINT { x = pointX, y = pointY };
        if (!onlyTargetWindow)
        {
            ClientToScreen(Handle, ref pt);
            hdc = default;
        }

        using var screenDC = GetDC_SafeHandle(hdc);
        using var compatibleDC = CreateCompatibleDC(screenDC);
        using var bmp = CreateCompatibleBitmap(screenDC, width, height);
        HGDIOBJ oldBmp = SelectObject(compatibleDC, (HGDIOBJ)bmp.DangerousGetHandle());

        BitBlt(compatibleDC, 0, 0, width, height, screenDC, pt.x, pt.y, ROP_CODE.SRCCOPY);
        SelectObject(compatibleDC, oldBmp);

        var image = Imaging.CreateBitmapSourceFromHBitmap(bmp.DangerousGetHandle(), default, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
        image.Freeze();
        return image;
    }
}