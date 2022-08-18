using KzLibraries.EventHandlerHistory;

namespace WindowScreenshot;

public class NaturalComparerTest
{
    private readonly NaturalComparer comparer= NaturalComparer.Default;
    [Theory]
    [InlineData(null, null)]
    [InlineData(null, "foo")]
    [InlineData("bar", "foo")]
    [InlineData("foo2", "foo10")]
    public void Compare(string first, string second)
    {
        if (first == second)
        {
            comparer.Compare(first, second).Should().Be(0);
            comparer.Compare(second, first).Should().Be(0);
        }
        else
        {
            comparer.Compare(first, second).Should().BeLessThan(0);
            comparer.Compare(second, first).Should().BeGreaterThan(0);
        }
    }
}