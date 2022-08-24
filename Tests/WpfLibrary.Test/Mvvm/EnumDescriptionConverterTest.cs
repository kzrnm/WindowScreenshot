using System.ComponentModel;
using System.Globalization;

namespace Kzrnm.Wpf.Mvvm;
public class EnumValuesWithDescriptionTextTest
{
    [Theory]
    [InlineData(OnePiece.Luffy, "Mugiwara")]
    [InlineData(OnePiece.Shanks, "Akagami")]
    public void Convert(OnePiece kind, string description)
    {
        new EnumDescriptionConverter().Convert(kind, typeof(OnePiece), null!, CultureInfo.InvariantCulture)
            .Should()
            .Be(description);
    }

    public enum OnePiece
    {
        [Description("Mugiwara")]
        Luffy,
        [Description("Akagami")]
        Shanks,
    }
}
