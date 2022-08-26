using Kzrnm.TwitterJikkyo.Models.Message;
using Kzrnm.Wpf.Behaviors;
using Kzrnm.Wpf.Views;
using System.Diagnostics;

namespace Kzrnm.TwitterJikkyo.Behaviors;
public class TwitetrAuthDialogBehaviour : TextInputDialogBehaviorBase<TwitetrAuthDialogBehaviour, TwitetrAuthDialogMessage>
{
    public override TextInputDialogWindow Initialize(TwitetrAuthDialogMessage message)
    {
        Process.Start(new ProcessStartInfo
        {
            UseShellExecute = true,
            FileName = message.Url,
        });
        return base.Initialize(message);
    }
}
