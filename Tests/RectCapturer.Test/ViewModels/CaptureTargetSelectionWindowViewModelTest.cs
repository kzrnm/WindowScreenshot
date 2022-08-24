using Kzrnm.RectCapturer.Configs;
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
            new Config(WindowPosition: new(1, 2, 3, 4)),
            new Shortcuts()
        ));
    }

    [Fact]
    public void Topmost()
    {
        var viewModel = new ConfigWindowViewModel(new(Topmost: true), new());
        viewModel.IsUpdated.Should().BeFalse();
        viewModel.Topmost.Should().BeTrue();
        viewModel.ToResult().Should().Be((
            new Config(Topmost: true),
            new Shortcuts()
        ));

        viewModel.Topmost = true;
        viewModel.IsUpdated.Should().BeFalse();

        viewModel.Topmost = false;
        viewModel.IsUpdated.Should().BeTrue();
        viewModel.Topmost.Should().BeFalse();
        viewModel.ToResult().Should().Be((
            new Config(Topmost: false),
            new Shortcuts()
        ));

        viewModel = new ConfigWindowViewModel(new(Topmost: false), new());
        viewModel.IsUpdated.Should().BeFalse();
        viewModel.Topmost.Should().BeFalse();
        viewModel.ToResult().Should().Be((
            new Config(Topmost: false),
            new Shortcuts()
        ));

        viewModel.Topmost = false;
        viewModel.IsUpdated.Should().BeFalse();

        viewModel.Topmost = true;
        viewModel.IsUpdated.Should().BeTrue();
        viewModel.Topmost.Should().BeTrue();
        viewModel.ToResult().Should().Be((
            new Config(Topmost: true),
            new Shortcuts()
        ));
    }

    [Fact]
    public void ShortcutPost()
    {
        var viewModel = new ConfigWindowViewModel(new(), new(Post: new(Control | Alt, Space)));
        viewModel.IsUpdated.Should().BeFalse();
        viewModel.ShortcutPost.Should().Be(new ShortcutKey(Control | Alt, Space));
        viewModel.ToResult().Should().Be((
            new Config(),
            new Shortcuts(Post: new(Control | Alt, Space))
        ));

        viewModel.ShortcutPost = new(Control | Alt, Space);
        viewModel.IsUpdated.Should().BeFalse();

        viewModel.ShortcutPost = new(Shift, Enter);
        viewModel.IsUpdated.Should().BeTrue();
        viewModel.ShortcutPost.Should().Be(new ShortcutKey(Shift, Enter));
        viewModel.ToResult().Should().Be((
            new Config(),
            new Shortcuts(Post: new(Shift, Enter))
        ));
    }

    [Fact]
    public void ShortcutCaptureScreenshot()
    {
        var viewModel = new ConfigWindowViewModel(new(), new(CaptureScreenshot: new(Control | Alt, Space)));
        viewModel.IsUpdated.Should().BeFalse();
        viewModel.ShortcutCaptureScreenshot.Should().Be(new ShortcutKey(Control | Alt, Space));
        viewModel.ToResult().Should().Be((
            new Config(),
            new Shortcuts(CaptureScreenshot: new(Control | Alt, Space))
        ));

        viewModel.ShortcutCaptureScreenshot = new(Control | Alt, Space);
        viewModel.IsUpdated.Should().BeFalse();

        viewModel.ShortcutCaptureScreenshot = new(Shift, Enter);
        viewModel.IsUpdated.Should().BeTrue();
        viewModel.ShortcutCaptureScreenshot.Should().Be(new ShortcutKey(Shift, Enter));
        viewModel.ToResult().Should().Be((
            new Config(),
            new Shortcuts(CaptureScreenshot: new(Shift, Enter))
        ));
    }

    [Fact]
    public void RestoreDefaultConfig()
    {
        var viewModel = new ConfigWindowViewModel(
            new(WindowPosition: new(1, 2, 3, 4), Topmost: true),
            new(Post: new(Control | Alt, Space), CaptureScreenshot: new(System.Windows.Input.ModifierKeys.Windows, D7)));

        viewModel.RestoreDefaultConfig();
        viewModel.IsUpdated.Should().BeTrue();
        viewModel.ToResult().Should().Be((
            new Config(
                WindowPosition: new(1, 2, 3, 4),
                Topmost: false
            ),
            new Shortcuts(
                Post: new(Control, Enter),
                CaptureScreenshot: new(Control, B)
            )
        ));
    }
}