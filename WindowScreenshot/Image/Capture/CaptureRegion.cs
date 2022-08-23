using Windows.Win32.Foundation;

namespace Kzrnm.WindowScreenshot.Image.Capture;

/// <summary>
/// キャプチャ対象の領域
/// </summary>
/// <param name="Left">ウインドウでの領域の左端の位置</param>
/// <param name="Right">ウインドウでの領域の右端の位置</param>
/// <param name="Top">ウインドウでの領域の上端の位置</param>
/// <param name="Bottom">ウインドウでの領域の下端の位置</param>
/// <param name="Width">領域の幅</param>
/// <param name="Height">領域の高さ</param>
/// <param name="UseRect">true なら領域を幅・高さで指定する。false なら右端・下端で指定する</param>
public record CaptureRegion(
    int Left,
    int Right,
    int Top,
    int Bottom,
    int Width,
    int Height,
    bool UseRect)
{
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
}
