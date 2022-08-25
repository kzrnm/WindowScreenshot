using Kzrnm.WindowScreenshot.Image;
using Kzrnm.WindowScreenshot.Image.Capture;
using Kzrnm.WindowScreenshot.Models;
using Kzrnm.WindowScreenshot.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Imaging;

namespace Kzrnm.RectCapturer.Models;
public class GlobalService : IGlobalService
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
    public IEnumerable<CaptureTarget> GetCaptureTargetWindows() => ConfigMaster.CaptureTargetWindows.Value;
}
