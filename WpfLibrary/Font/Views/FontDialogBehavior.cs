using Kzrnm.Wpf.Behaviors;

namespace Kzrnm.Wpf.Font.Views;
public class FontDialogBehavior : DialogBehaviorBase<FontDialogBehavior, FontDialogMessage>
{
    public override void Receive(FontDialogMessage message)
    {
        var dialog = new FontDialogWindow(message.InitialValue)
        {
            Owner = GetWindow(),
        };

        message.Reply(dialog.ShowDialogWithResponse());
    }
}