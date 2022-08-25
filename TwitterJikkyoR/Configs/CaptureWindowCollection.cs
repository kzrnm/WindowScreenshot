using Kzrnm.WindowScreenshot.Image.Capture;
using System.Collections.Generic;

namespace Kzrnm.TwitterJikkyo.Configs;

public class CaptureWindowCollection : List<CaptureTarget>
{
    public CaptureWindowCollection() : base() { }
    public CaptureWindowCollection(IEnumerable<CaptureTarget> collection) : base(collection) { }
}
