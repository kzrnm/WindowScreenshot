using CommunityToolkit.Mvvm.Messaging.Messages;

namespace WindowScreenshot.Image;
public class ImageCountChangedMessage : ValueChangedMessage<int>
{
    public ImageCountChangedMessage(int value) : base(value)
    {
    }
}
