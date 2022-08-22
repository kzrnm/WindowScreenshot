using CommunityToolkit.Mvvm.Messaging.Messages;

namespace Kzrnm.WindowScreenshot.Image;
public class ImageCountChangedMessage : ValueChangedMessage<int>
{
    public ImageCountChangedMessage(int value) : base(value)
    {
    }
}
