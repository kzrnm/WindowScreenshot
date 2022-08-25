using Kzrnm.TwitterJikkyo.Views;
using Kzrnm.WindowScreenshot.Models;
using Kzrnm.Wpf.Behaviors;

namespace Kzrnm.TwitterJikkyo.Behaviors;
public class ConfigDialogBehavior : DialogBehaviorBase<ConfigDialogBehavior, ConfigDialogMessage>
{
    public override void Receive(ConfigDialogMessage message)
    {
        if (message.InitialValue is not { } tup)
        {
            message.Reply(null);
            return;
        }
        var (config, shortcuts) = tup;
        var dialog = new ConfigWindow(config, shortcuts)
        {
            Owner = GetWindow(),
        };

        message.Reply(dialog.ShowDialogWithResponse());
    }
}
