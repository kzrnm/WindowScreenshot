using Kzrnm.TwitterJikkyo.Properties;
using Kzrnm.Wpf.Models;

namespace Kzrnm.TwitterJikkyo.Models.Message;
public class TwitetrAuthDialogMessage : TextInputDialogMessage
{
    public TwitetrAuthDialogMessage(string url) : base("")
    {
        Url = url;

        Title = Resources.Authentication;
        Message = Resources.PinCode;
        MinWidth = 200;
    }

    public string Url { get; }
}
