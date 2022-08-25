using Kzrnm.Wpf.Behaviors;
using System.Windows;

namespace Kzrnm.Wpf.Font.Views;
public class FontDialogBehavior : DialogBehaviorBase<FontDialogBehavior, FontDialogMessage>
{
    public override void Receive(FontDialogMessage message)
    {
        var dialog = new FontDialogWindow(message.InitialValue)
        {
            Owner = GetWindow(),
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
        };

        message.Reply(dialog.ShowDialogWithResponse());
    }
}