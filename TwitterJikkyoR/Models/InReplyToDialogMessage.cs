using Kzrnm.TwitterJikkyo.Properties;
using Kzrnm.Wpf.Models;

namespace Kzrnm.WindowScreenshot.Models;
public class InReplyToDialogMessage : TextInputDialogMessage
{
    public InReplyToDialogMessage(string initialValue) : base(initialValue)
    {
        Title = Resources.InputInReplyTo;
        Message = Resources.InputInReplyToMessage;

        MinWidth = 300;
    }
}
