using CommunityToolkit.Mvvm.Messaging.Messages;
using System.Collections.Generic;

namespace Kzrnm.WindowScreenshot.Image.Capture;
public class CurrentWindowProcessHandlesMessage : ValueChangedMessage<IEnumerable<IWindowProcessHandle>>
{
    public CurrentWindowProcessHandlesMessage(IEnumerable<IWindowProcessHandle> value) : base(value)
    {
    }
}