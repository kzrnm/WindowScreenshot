using CommunityToolkit.Mvvm.Messaging;
using Kzrnm.WindowScreenshot.Models;
using Microsoft.Xaml.Behaviors;
using System.Windows;
using System.Windows.Controls;

namespace Kzrnm.WindowScreenshot.Behaviors;
public class DialogBehavior : Behavior<Control>
{
    protected override void OnAttached()
    {
        WeakReferenceMessenger.Default.Register<DialogBehavior, DialogMessage>(this, OnMessage);
    }

    protected override void OnDetaching()
    {
        WeakReferenceMessenger.Default.Unregister<DialogMessage>(this);
    }

    private void OnMessage(DialogBehavior recipient, DialogMessage message)
    {
        var text = message.Text ?? "";
        var caption = message.Caption ?? "";
        var result = MessageBox.Show(Window.GetWindow(AssociatedObject), text, caption, message.MessageBoxButton, message.MessageBoxImage);

        message.Reply(result);
    }
}