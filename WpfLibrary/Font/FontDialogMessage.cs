using CommunityToolkit.Mvvm.Messaging.Messages;

namespace Kzrnm.Wpf.Font;

public class FontDialogMessage : RequestMessage<FontDialogParams?>
{
    public FontDialogMessage(FontDialogParams initialValue)
    {
        InitialValue = initialValue;
    }
    public FontDialogParams InitialValue { get; }
}