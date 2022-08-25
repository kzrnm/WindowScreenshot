using Kzrnm.WindowScreenshot.Image;

namespace Kzrnm.TwitterJikkyo.Models;

public class TweetImageProvider : ImageProvider
{
    public override bool CanAddImage => Images.Count <= 4;
}
