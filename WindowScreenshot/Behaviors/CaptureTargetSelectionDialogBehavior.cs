using Kzrnm.WindowScreenshot.Models;
using Kzrnm.WindowScreenshot.Views;
using Kzrnm.Wpf.Behaviors;

namespace Kzrnm.WindowScreenshot.Behaviors;
public class CaptureTargetSelectionDialogBehavior : DialogBehaviorBase<CaptureTargetSelectionDialogBehavior, CaptureTargetSelectionDialogMessage>
{
    public override void Receive(CaptureTargetSelectionDialogMessage message)
    {
        var dialog = new CaptureTargetSelectionWindow(message.InitialValue)
        {
            Owner = GetWindow(),
        };

        message.Reply(dialog.ShowDialogWithResponse());
    }
}