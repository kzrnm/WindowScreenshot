using Kzrnm.WindowScreenshot.Image;
using Kzrnm.WindowScreenshot.Image.Capture;
using Kzrnm.WindowScreenshot.Windows;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Kzrnm.RectCapturer.Models;
public class GlobalService
{
    public GlobalService(ConfigMaster configMaster, ImageProvider imageProvider, IObserveWindowProcess observeWindowProcess, IClipboardManager clipboardManager)
    {
        ConfigMaster = configMaster;
        ImageProvider = imageProvider;
        ObserveWindowProcess = observeWindowProcess;
        ClipboardManager = clipboardManager;
    }
    public ConfigMaster ConfigMaster { get; }
    public ImageProvider ImageProvider { get; }
    public IObserveWindowProcess ObserveWindowProcess { get; }
    public IClipboardManager ClipboardManager { get; }
    public bool CanPasteImageFromClipboard => ImageProvider.CanAddImage && ClipboardManager.ContainsImage();
    public void PasteImageFromClipboard()
    {
        if (ClipboardManager.GetImage() is { } image)
            ImageProvider.AddImage(image);
    }
    public void CaptureScreenshot()
    {
        var image = ConfigMaster.CaptureTargetWindows.Value
            .Select(ct => ct.CaptureFrom(ObserveWindowProcess.CurrentWindows))
            .OfType<BitmapSource>()
            .FirstOrDefault();

        if (image is not null)
        {
            ImageProvider.AddImage(image);
        }
    }
}
