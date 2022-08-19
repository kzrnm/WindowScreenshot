using Kzrnm.EventHandlerHistory;

namespace Kzrnm.WindowScreenshot;

using SelectedItemChangedEventArgs = SelectorObservableCollection<string>.SelectedItemChangedEventArgs;
public class SelectorObservableCollectionTests
{
    readonly SelectorObservableCollection<string> collection;
    readonly List<SelectedItemChangedEventArgs> selectedChangedHistory;
    readonly PropertyChangedHistory properyChangedHistory;
    public SelectorObservableCollectionTests()
    {
        collection = new();
        selectedChangedHistory = new();
        properyChangedHistory = new(collection);
        collection.SelectedChanged += (_, e) => selectedChangedHistory.Add(e);
    }

    [Fact]
    public void AddAndInsert()
    {
        selectedChangedHistory.Should().BeEmpty();
        collection.SelectedIndex.Should().Be(-1);
        collection.SelectedItem.Should().Be(null);
        collection.Should().BeEmpty();

        collection.Add("foo");
        collection.Should().Equal("foo");
        selectedChangedHistory.Should().ContainSingle().Which.Should().BeEquivalentTo(new SelectedItemChangedEventArgs(-1, 0, null, "foo"));
        collection.SelectedIndex.Should().Be(0);
        collection.SelectedItem.Should().Be("foo");

        collection.Add("bar");
        collection.Should().Equal("foo", "bar");
        selectedChangedHistory.Should().BeEquivalentTo(
            new[]{
            new SelectedItemChangedEventArgs(-1, 0, null, "foo"),
            new SelectedItemChangedEventArgs(0, 1, "foo", "bar"),
            });
        collection.SelectedIndex.Should().Be(1);
        collection.SelectedItem.Should().Be("bar");

        collection.Insert(1, "baz");
        collection.Should().Equal("foo", "baz", "bar");
        selectedChangedHistory.Should().BeEquivalentTo(
            new[]{
            new SelectedItemChangedEventArgs(-1, 0, null, "foo"),
            new SelectedItemChangedEventArgs(0, 1, "foo", "bar"),
            new SelectedItemChangedEventArgs(1, 1, "bar", "baz"),
        });
        collection.SelectedIndex.Should().Be(1);
        collection.SelectedItem.Should().Be("baz");
    }

    [Fact]
    public void Remove()
    {
        collection.Add("foo");
        collection.Add("bar");
        collection.Add("baz");
        collection.Add("fizz");
        collection.Add("buzz");
        collection.SelectedIndex = -1;
        selectedChangedHistory.Clear();
        properyChangedHistory.Clear();

        collection.SelectedIndex.Should().Be(-1);
        collection.SelectedItem.Should().Be(null);
        collection.Should().Equal("foo", "bar", "baz", "fizz", "buzz");
        selectedChangedHistory.Should().BeEmpty();

        collection.RemoveSelectedItem();
        collection.SelectedIndex.Should().Be(-1);
        collection.SelectedItem.Should().Be(null);
        collection.Should().Equal("foo", "bar", "baz", "fizz", "buzz");
        selectedChangedHistory.Should().BeEmpty();

        collection.Remove("fizz");
        collection.SelectedIndex.Should().Be(-1);
        collection.SelectedItem.Should().Be(null);
        collection.Should().Equal("foo", "bar", "baz", "buzz");
        selectedChangedHistory.Should().BeEmpty();

        collection.SelectedIndex = 3;
        selectedChangedHistory.Clear();
        properyChangedHistory.Clear();
        collection.Remove("buzz");
        collection.SelectedIndex.Should().Be(2);
        collection.SelectedItem.Should().Be("baz");
        collection.Should().Equal("foo", "bar", "baz");
        
        selectedChangedHistory.Should().ContainSingle().Which.Should().BeEquivalentTo(new SelectedItemChangedEventArgs(3, 2, "buzz", "baz"));

        collection.SelectedIndex = 1;
        selectedChangedHistory.Clear();
        properyChangedHistory.Clear();
        collection.RemoveAt(0);
        collection.SelectedIndex.Should().Be(0);
        collection.SelectedItem.Should().Be("bar");
        collection.Should().Equal("bar", "baz");
        
        selectedChangedHistory.Should().ContainSingle().Which.Should().BeEquivalentTo(new SelectedItemChangedEventArgs(1, 0, "bar", "bar"));

        selectedChangedHistory.Clear();
        properyChangedHistory.Clear();
        collection.RemoveAt(0);
        collection.SelectedIndex.Should().Be(0);
        collection.SelectedItem.Should().Be("baz");
        collection.Should().Equal("baz");
        
        selectedChangedHistory.Should().ContainSingle().Which.Should().BeEquivalentTo(new SelectedItemChangedEventArgs(0, 0, "bar", "baz"));

        selectedChangedHistory.Clear();
        properyChangedHistory.Clear();
        collection.RemoveSelectedItem();
        collection.SelectedIndex.Should().Be(-1);
        collection.SelectedItem.Should().Be(null);
        collection.Should().BeEmpty();
        
        selectedChangedHistory.Should().ContainSingle().Which.Should().BeEquivalentTo(new SelectedItemChangedEventArgs(0, -1, "baz", null));
    }

    [Fact]
    public void Clear()
    {
        collection.Add("foo");
        collection.Add("bar");
        collection.Add("baz");
        collection.Add("fizz");
        collection.Add("buzz");
        collection.Should().Equal("foo", "bar", "baz", "fizz", "buzz");
        collection.SelectedIndex = -1;
        selectedChangedHistory.Clear();
        properyChangedHistory.Clear();

        collection.Clear();
        collection.SelectedIndex.Should().Be(-1);
        collection.SelectedItem.Should().Be(null);
        collection.Should().BeEmpty();
        selectedChangedHistory.Should().BeEmpty();


        collection.Add("foo");
        collection.Add("bar");
        collection.Add("baz");
        collection.Add("fizz");
        collection.Add("buzz");
        collection.Should().Equal("foo", "bar", "baz", "fizz", "buzz");
        collection.SelectedIndex = 0;
        selectedChangedHistory.Clear();
        properyChangedHistory.Clear();

        collection.Clear();
        collection.SelectedIndex.Should().Be(-1);
        collection.SelectedItem.Should().Be(null);
        collection.Should().BeEmpty();
        
        selectedChangedHistory.Should().ContainSingle().Which.Should().BeEquivalentTo(new SelectedItemChangedEventArgs(0, -1, "foo", null));
    }

    [Fact]
    public void Move()
    {
        collection.Add("foo");
        collection.Add("bar");
        collection.Add("baz");
        collection.Add("fizz");
        collection.Add("buzz");
        collection.Should().Equal("foo", "bar", "baz", "fizz", "buzz");
        collection.SelectedIndex = -1;
        selectedChangedHistory.Clear();
        properyChangedHistory.Clear();

        collection.Move(1, 3);
        collection.SelectedIndex.Should().Be(-1);
        collection.SelectedItem.Should().Be(null);
        collection.Should().Equal("foo", "baz", "fizz", "bar", "buzz");
        selectedChangedHistory.Should().BeEmpty();

        collection.SelectedIndex = 2;
        collection.SelectedIndex.Should().Be(2);
        collection.SelectedItem.Should().Be("fizz");
        selectedChangedHistory.Clear();
        properyChangedHistory.Clear();
        collection.Move(2, 3);
        collection.SelectedIndex.Should().Be(3);
        collection.SelectedItem.Should().Be("fizz");
        collection.Should().Equal("foo", "baz", "bar", "fizz", "buzz");
        
        selectedChangedHistory.Should().ContainSingle().Which.Should().BeEquivalentTo(new SelectedItemChangedEventArgs(2, 3, "fizz", "fizz"));
    }


    [Fact]
    public void SetItem()
    {
        collection.Add("foo");
        collection.Add("bar");
        collection.Add("baz");
        collection.Add("fizz");
        collection.Add("buzz");
        collection.Should().Equal("foo", "bar", "baz", "fizz", "buzz");
        collection.SelectedIndex = -1;
        selectedChangedHistory.Clear();
        properyChangedHistory.Clear();

        collection[0] = "fx";
        collection.Should().Equal("fx", "bar", "baz", "fizz", "buzz");
        selectedChangedHistory.Should().BeEmpty();

        collection.SelectedIndex = 2;
        selectedChangedHistory.Clear();
        properyChangedHistory.Clear();
        collection[2] = "bx";
        collection.Should().Equal("fx", "bar", "bx", "fizz", "buzz");

        selectedChangedHistory.Should().ContainSingle().Which.Should().BeEquivalentTo(new SelectedItemChangedEventArgs(2, 2, "baz", "bx"));
    }
}