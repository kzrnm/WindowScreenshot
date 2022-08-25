using CommunityToolkit.Mvvm.Messaging;
using Kzrnm.WindowScreenshot.Models;
using Kzrnm.WindowScreenshot.Views;
using Microsoft.Xaml.Behaviors;
using System.Windows;
using System.Windows.Controls;

namespace Kzrnm.WindowScreenshot.Behaviors;
public class CaptureTargetSelectionDialogBehavior : Behavior<Window>
{
    protected override void OnAttached()
    {
        WeakReferenceMessenger.Default.Register<CaptureTargetSelectionDialogBehavior, CaptureTargetSelectionDialogMessage>(this, OnMessage);
        AssociatedObject.Closed += (_, _) => Detach();
    }

    protected override void OnDetaching()
    {
        WeakReferenceMessenger.Default.Unregister<CaptureTargetSelectionDialogMessage>(this);
    }

    private void OnMessage(CaptureTargetSelectionDialogBehavior recipient, CaptureTargetSelectionDialogMessage message)
    {
        var dialog = new CaptureTargetSelectionWindow(message.InitialValue)
        {
            Owner = AssociatedObject,
        };

        message.Reply(dialog.ShowDialogWithResponse());
    }
}