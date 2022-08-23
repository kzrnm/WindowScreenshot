using System.Globalization;

namespace Kzrnm.WindowScreenshot.Mvvm;
public class InverseBooleanConverterTest
{
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void Convert(bool value)
    {
        new InverseBooleanConverter().Convert(value, typeof(bool), null!, CultureInfo.InvariantCulture)
            .Should()
            .Be(!value);
    }
}
