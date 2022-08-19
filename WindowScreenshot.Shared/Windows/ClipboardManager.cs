using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Kzrnm.WindowScreenshot.Windows;

public interface IClipboardManager
{
    public void SetImage(BitmapSource image);
    public BitmapSource GetImage();
    public bool ContainsImage();
}
public class ClipboardManager : IClipboardManager
{
    public ClipboardManager() { }
    public void SetImage(BitmapSource image)
    {
        try
        {
            Clipboard.SetImage(image);
        }
        catch (COMException e) when (e.ErrorCode is unchecked((int)0x800401D0))
        {
            
        }
    }
    public BitmapSource GetImage() => Clipboard.GetImage();
    public bool ContainsImage() => Clipboard.ContainsImage();
}
