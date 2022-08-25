using Kzrnm.WindowScreenshot.Models;
using Kzrnm.WindowScreenshot.Views;
using Kzrnm.Wpf.Behaviors;
using System.Windows;

namespace Kzrnm.WindowScreenshot.Behaviors;
public class CaptureTargetSelectionDialogBehavior : DialogBehaviorBase<CaptureTargetSelectionDialogBehavior, CaptureTargetSelectionDialogMessage>
{
    public override void Receive(CaptureTargetSelectionDialogMessage message)
    {
        var dialog = new CaptureTargetSelectionWindow(message.InitialValue)
        {
            Owner = GetWindow(),
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
        };

        message.Reply(dialog.ShowDialogWithResponse());
    }
}