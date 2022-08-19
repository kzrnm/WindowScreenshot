using CommunityToolkit.Mvvm.Messaging.Messages;

namespace Kzrnm.Wpf.Font;

public class FontDialogMessage : RequestMessage<Font?>
{
    public FontDialogMessage(Font initialValue)
    {
        InitialValue = initialValue;
    }
    public Font InitialValue { get; }
}