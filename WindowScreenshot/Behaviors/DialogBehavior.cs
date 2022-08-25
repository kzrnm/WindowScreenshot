using Kzrnm.WindowScreenshot.Models;
using Kzrnm.Wpf.Behaviors;
using System.Windows;

namespace Kzrnm.WindowScreenshot.Behaviors;
public class DialogBehavior : DialogBehaviorBase<DialogBehavior, DialogMessage>
{
    public override void Receive(DialogMessage message)
    {
        var text = message.Text ?? "";
        var caption = message.Caption ?? "";
        var result = MessageBox.Show(GetWindow(), text, caption, message.MessageBoxButton, message.MessageBoxImage);
        message.Reply(result);
    }
}