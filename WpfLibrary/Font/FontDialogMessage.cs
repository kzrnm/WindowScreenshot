using Kzrnm.Wpf.Mvvm;

namespace Kzrnm.Wpf.Font;

public class FontDialogMessage : InitializedRequestMessage<Font>
{
    public FontDialogMessage(Font initialValue) : base(initialValue)
    {
    }
}