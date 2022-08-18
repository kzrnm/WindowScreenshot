using CommunityToolkit.Mvvm.Messaging.Messages;

namespace WindowScreenshot.Image;

public class SelectedImageChangedMessage : ValueChangedMessage<CaptureImage?>
{
    public SelectedImageChangedMessage(CaptureImage? value) : base(value)
    {
    }
}
