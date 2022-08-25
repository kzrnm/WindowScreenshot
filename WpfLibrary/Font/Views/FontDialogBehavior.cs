using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Xaml.Behaviors;
using System.Windows;
using System.Windows.Controls;

namespace Kzrnm.Wpf.Font.Views;
public class FontDialogBehavior : Behavior<Window>
{
    protected override void OnAttached()
    {
        WeakReferenceMessenger.Default.Register<FontDialogBehavior, FontDialogMessage>(this, OnMessage);
        AssociatedObject.Closed += (_, _) => Detach();
    }

    protected override void OnDetaching()
    {
        WeakReferenceMessenger.Default.Unregister<FontDialogMessage>(this);
    }

    private void OnMessage(FontDialogBehavior recipient, FontDialogMessage message)
    {
        var dialog = new FontDialogWindow(message.InitialValue)
        {
            Owner = AssociatedObject,
        };

        message.Reply(dialog.ShowDialogWithResponse());
    }
}