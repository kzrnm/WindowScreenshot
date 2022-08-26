using Kzrnm.TwitterJikkyo.Models.Message;
using Kzrnm.TwitterJikkyo.Views;
using Kzrnm.Wpf.Behaviors;
using System.Windows;

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
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
        };

        message.Reply(dialog.ShowDialogWithResponse());
    }
}
