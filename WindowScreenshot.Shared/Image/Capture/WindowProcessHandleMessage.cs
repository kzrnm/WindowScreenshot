using CommunityToolkit.Mvvm.Messaging.Messages;

namespace WindowScreenshot.Image.Capture;
public class CurrentWindowProcessHandlesMessage : ValueChangedMessage<WindowProcessHandle[]>
{
    public CurrentWindowProcessHandlesMessage(WindowProcessHandle[] value) : base(value)
    {
    }
}