using System;

namespace Windows.Win32;
using Foundation;

// https://github.com/microsoft/CsWin32/issues/209

internal sealed class ReleaseDCSafeHandle : DeleteDCSafeHandle
{
    public ReleaseDCSafeHandle(HWND hWnd, IntPtr handle)
        : base(handle)
    {
        HWnd = hWnd;
    }

    public HWND HWnd { get; }

    protected override bool ReleaseHandle() => PInvoke.ReleaseDC(HWnd, (Graphics.Gdi.HDC)handle) != 0;
}