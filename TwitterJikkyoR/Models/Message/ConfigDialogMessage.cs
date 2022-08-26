using Kzrnm.TwitterJikkyo.Configs;
using Kzrnm.Wpf.Mvvm;

namespace Kzrnm.TwitterJikkyo.Models.Message;
public class ConfigDialogMessage : InitializedRequestMessage<(Config Config, Shortcuts Shortcuts)?>
{
    public ConfigDialogMessage(Config config, Shortcuts shortcuts) : base((config, shortcuts))
    {
    }
}
