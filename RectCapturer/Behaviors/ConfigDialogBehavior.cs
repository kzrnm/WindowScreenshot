using CommunityToolkit.Mvvm.Messaging;
using Kzrnm.RectCapturer.Views;
using Kzrnm.WindowScreenshot.Models;
using Microsoft.Xaml.Behaviors;
using System.Windows;

namespace Kzrnm.RectCapturer.Behaviors;
public class ConfigDialogBehavior : Behavior<Window>
{
    protected override void OnAttached()
    {
        WeakReferenceMessenger.Default.Register<ConfigDialogBehavior, ConfigDialogMessage>(this, OnMessage);
        AssociatedObject.Closed += (_, _) => Detach();
    }

    protected override void OnDetaching()
    {
        WeakReferenceMessenger.Default.Unregister<ConfigDialogMessage>(this);
    }

    private void OnMessage(ConfigDialogBehavior recipient, ConfigDialogMessage message)
    {
        if (message.InitialValue is not { } tup)
        {
            message.Reply(null);
            return;
        }
        var (config, shortcuts) = tup;
        var dialog = new ConfigWindow(config, shortcuts)
        {
            Owner = AssociatedObject,
        };

        message.Reply(dialog.ShowDialogWithResponse());
    }
}
