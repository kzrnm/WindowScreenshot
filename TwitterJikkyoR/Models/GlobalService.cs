using Kzrnm.WindowScreenshot;
using Kzrnm.WindowScreenshot.Image;
using Kzrnm.WindowScreenshot.Image.Capture;
using Kzrnm.WindowScreenshot.Models;
using Kzrnm.WindowScreenshot.Windows;
using System.Collections.Generic;

namespace Kzrnm.TwitterJikkyo.Models;
public class GlobalService : IGlobalService
{
    public GlobalService(ConfigMaster configMaster, ImageProvider imageProvider, IObserveWindowProcess observeWindowProcess, IClipboardManager clipboardManager, AccountService accountService)
    {
        ConfigMaster = configMaster;
        ImageProvider = imageProvider;
        ObserveWindowProcess = observeWindowProcess;
        ClipboardManager = clipboardManager;
        AccountService = accountService;
        Hashtags = ConfigMaster.Hashtags.Value.PresetHashtags;
    }
    public ConfigMaster ConfigMaster { get; }
    public ImageProvider ImageProvider { get; }
    public IObserveWindowProcess ObserveWindowProcess { get; }
    public IClipboardManager ClipboardManager { get; }
    public IEnumerable<CaptureTarget> GetCaptureTargetWindows() => ConfigMaster.CaptureTargetWindows.Value;
    public AccountService AccountService { get; }
    public SelectorObservableCollection<string> Hashtags { get; }
}
