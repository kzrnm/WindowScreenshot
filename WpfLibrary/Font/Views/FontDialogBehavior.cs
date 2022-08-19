using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using Microsoft.Xaml.Behaviors;
using System.Windows;

namespace Kzrnm.Wpf.Font.Views;
public class FontDialogBehavior : Behavior<Window>
{
    protected override void OnAttached()
    {
        WeakReferenceMessenger.Default.Register<FontDialogBehavior, FontDialogMessage>(this, OnMessage);
    }

    protected override void OnDetaching()
    {
        WeakReferenceMessenger.Default.Unregister<FontDialogMessage>(this);
    }

    private void OnMessage(FontDialogBehavior recipient, FontDialogMessage message)
    {
        var dialog = new FontDialogWindow(message.InitialValue)
        {
            Owner = Window.GetWindow(AssociatedObject),
        };

        var response = dialog.ShowDialog() is true ? dialog.SelectedFont : null;
        message.Reply(response);
    }
}