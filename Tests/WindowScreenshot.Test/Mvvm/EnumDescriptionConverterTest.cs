using Kzrnm.WindowScreenshot.Image;
using System.Globalization;

namespace Kzrnm.WindowScreenshot.Mvvm;
public class EnumDescriptionConverterTest
{
    [Theory]
    [InlineData(ImageKind.Jpg, "jpeg")]
    [InlineData(ImageKind.Png, "png")]
    public void Convert(ImageKind kind, string description)
    {
        new EnumDescriptionConverter().Convert(kind, typeof(ImageKind), null!, CultureInfo.InvariantCulture)
            .Should()
            .Be(description);
    }
}
