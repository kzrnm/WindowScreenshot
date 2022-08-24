using Kzrnm.RectCapturer.Configs;
using Kzrnm.Wpf.Mvvm;

namespace Kzrnm.WindowScreenshot.Models;
public class ConfigDialogMessage : InitializedRequestMessage<(Config Config, Shortcuts Shortcuts)?>
{
    public ConfigDialogMessage(Config config, Shortcuts shortcuts) : base((config, shortcuts))
    {
    }
}
