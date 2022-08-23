using Kzrnm.WindowScreenshot.Image.Capture;
using Kzrnm.Wpf.Mvvm;
using System.Collections.Generic;

namespace Kzrnm.WindowScreenshot.Models;
public class CaptureTargetSelectionDialogMessage : InitializedRequestMessage<IList<CaptureTarget>>
{
    public CaptureTargetSelectionDialogMessage(IList<CaptureTarget> initialValue) : base(initialValue)
    {
    }
}
