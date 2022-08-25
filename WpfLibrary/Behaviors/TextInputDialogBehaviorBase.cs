using Kzrnm.Wpf.Models;
using Kzrnm.Wpf.Views;
using System.Windows;

namespace Kzrnm.Wpf.Behaviors;

public abstract class TextInputDialogBehaviorBase<TSelf, TMessage> : DialogBehaviorBase<TSelf, TMessage>
    where TSelf : TextInputDialogBehaviorBase<TSelf, TMessage>
    where TMessage : TextInputDialogMessage
{
    public override void Receive(TMessage message)
    {
        var dialog = new TextInputDialogWindow()
        {
            Owner = GetWindow(),
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
        };

        dialog.InputTextBox.Text = message.InitialValue;

        if (message.Title is { } title)
            dialog.Title = title;
        if (message.Message is { } messageText)
            dialog.Message = messageText;

        if (message.Width is { } width)
            dialog.Width = width;
        if (message.Height is { } height)
            dialog.Height = height;
        if (message.MinWidth is { } minWidth)
            dialog.MinWidth = minWidth;
        if (message.MinHeight is { } minHeight)
            dialog.MinHeight = minHeight;

        message.Reply(dialog.ShowDialogWithResponse());
    }
}