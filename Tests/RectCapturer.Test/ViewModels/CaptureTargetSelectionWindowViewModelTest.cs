using Kzrnm.RectCapturer.Configs;
using Kzrnm.Wpf.Font;
using Kzrnm.Wpf.Input;
using static System.Windows.Input.Key;
using static System.Windows.Input.ModifierKeys;

namespace Kzrnm.RectCapturer.ViewModels;

public class ConfigWindowViewModelTest
{
    [Fact]
    public void ResultWindowPosition()
    {
        var viewModel = new ConfigWindowViewModel(new(WindowPosition: new(1, 2, 3, 4)), new());
        viewModel.ToResult().Should().Be((
            new(WindowPosition: new(1, 2, 3, 4)),
            new()
        ));
    }

    [Fact]
    public void Topmost()
    {
        var viewModel = new ConfigWindowViewModel(new(Topmost: true), new());
        viewModel.IsUpdated.Should().BeFalse();
        viewModel.Topmost.Should().BeTrue();
        viewModel.ToResult().Should().Be((
            new(Topmost: true),
            new()
        ));

        viewModel.Topmost = true;
        viewModel.IsUpdated.Should().BeFalse();

        viewModel.Topmost = false;
        viewModel.IsUpdated.Should().BeTrue();
        viewModel.Topmost.Should().BeFalse();
        viewModel.ToResult().Should().Be((
            new(Topmost: false),
            new()
        ));

        viewModel = new ConfigWindowViewModel(new(Topmost: false), new());
        viewModel.IsUpdated.Should().BeFalse();
        viewModel.Topmost.Should().BeFalse();
        viewModel.ToResult().Should().Be((
            new(Topmost: false),
            new()
        ));

        viewModel.Topmost = false;
        viewModel.IsUpdated.Should().BeFalse();

        viewModel.Topmost = true;
        viewModel.IsUpdated.Should().BeTrue();
        viewModel.Topmost.Should().BeTrue();
        viewModel.ToResult().Should().Be((
            new(Topmost: true),
            new()
        ));
    }

    [Fact]
    public void ShortcutPost()
    {
        var viewModel = new ConfigWindowViewModel(new(), new(Post: new(Control | Alt, Space)));
        viewModel.IsUpdated.Should().BeFalse();
        viewModel.ShortcutPost.Should().Be(new ShortcutKey(Control | Alt, Space));
        viewModel.ToResult().Should().Be((
            new(),
            new(Post: new(Control | Alt, Space))
        ));

        viewModel.ShortcutPost = new(Control | Alt, Space);
        viewModel.IsUpdated.Should().BeFalse();

        viewModel.ShortcutPost = new(Shift, Enter);
        viewModel.IsUpdated.Should().BeTrue();
        viewModel.ShortcutPost.Should().Be(new ShortcutKey(Shift, Enter));
        viewModel.ToResult().Should().Be((
            new(),
            new(Post: new(Shift, Enter))
        ));
    }

    [Fact]
    public void ShortcutCaptureScreenshot()
    {
        var viewModel = new ConfigWindowViewModel(new(), new(CaptureScreenshot: new(Control | Alt, Space)));
        viewModel.IsUpdated.Should().BeFalse();
        viewModel.ShortcutCaptureScreenshot.Should().Be(new ShortcutKey(Control | Alt, Space));
        viewModel.ToResult().Should().Be((
            new(),
            new(CaptureScreenshot: new(Control | Alt, Space))
        ));

        viewModel.ShortcutCaptureScreenshot = new(Control | Alt, Space);
        viewModel.IsUpdated.Should().BeFalse();

        viewModel.ShortcutCaptureScreenshot = new(Shift, Enter);
        viewModel.IsUpdated.Should().BeTrue();
        viewModel.ShortcutCaptureScreenshot.Should().Be(new ShortcutKey(Shift, Enter));
        viewModel.ToResult().Should().Be((
            new(),
            new(CaptureScreenshot: new(Shift, Enter))
        ));
    }

    [Fact]
    public void Font()
    {
        var viewModel = new ConfigWindowViewModel(new(Font: new("Meiryo", 16)), new());
        viewModel.IsUpdated.Should().BeFalse();
        viewModel.Font.Should().Be(new Font("Meiryo", 16));
        viewModel.ToResult().Should().Be((
            new(Font: new("Meiryo", 16)),
            new()
        ));

        viewModel.Font = new("Meiryo", 16);
        viewModel.IsUpdated.Should().BeFalse();

        viewModel.Font = new("Yu Gothic", 12);
        viewModel.IsUpdated.Should().BeTrue();
        viewModel.Font.Should().Be(new Font("Meiryo", 16));
        viewModel.ToResult().Should().Be((
            new(Font: new Font("Meiryo", 16)),
            new()
        ));
    }

    [Fact]
    public void RestoreDefaultConfig()
    {
        var viewModel = new ConfigWindowViewModel(
            new(
                WindowPosition: new(1, 2, 3, 4),
                Font: new("Meiryo", 16),
                Topmost: true
            ),
            new(Post: new(Control | Alt, Space), CaptureScreenshot: new(System.Windows.Input.ModifierKeys.Windows, D7)));

        viewModel.RestoreDefaultConfig();
        viewModel.IsUpdated.Should().BeTrue();
        viewModel.ToResult().Should().Be((
            new Config(
                WindowPosition: new(1, 2, 3, 4),
                Font: new(),
                Topmost: false
            ),
            new Shortcuts(
                Post: new(Control, Enter),
                CaptureScreenshot: new(Control, B)
            )
        ));
    }
}