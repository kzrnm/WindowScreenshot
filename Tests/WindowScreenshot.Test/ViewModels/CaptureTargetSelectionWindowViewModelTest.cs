using CommunityToolkit.Mvvm.Messaging;
using Kzrnm.EventHandlerHistory;
using Kzrnm.WindowScreenshot.Image.Capture;
using Moq;

namespace Kzrnm.WindowScreenshot.ViewModels;
using ObservableCaptureTarget = CaptureTargetSelectionWindowViewModel.ObservableCaptureTarget;

public class CaptureTargetSelectionWindowViewModelTest
{
    [Fact]
    public void UpdateSelected()
    {
        var messenger = new WeakReferenceMessenger();
        var selected = new ObservableCaptureTarget
        {
            ProcessName = "test1",
            WindowName = "win1",
        };
        var viewModel = new CaptureTargetSelectionWindowViewModel(messenger, Array.Empty<IWindowProcessHandle>(), Array.Empty<CaptureTarget>())
        {
            SelectedTarget = selected,
        };
        var ph = new PropertyChangedHistory(selected);

        viewModel.ProcessName.Should().Be("test1");
        viewModel.SelectedTarget.ProcessName.Should().Be("test1");
        viewModel.WindowName.Should().Be("win1");
        viewModel.SelectedTarget.WindowName.Should().Be("win1");
        viewModel.SelectedTarget.Should().BeSameAs(selected);

        viewModel.ProcessName = "test2";
        viewModel.ProcessName.Should().Be("test2");
        viewModel.SelectedTarget.ProcessName.Should().Be("test2");
        viewModel.WindowName.Should().Be("win1");
        viewModel.SelectedTarget.WindowName.Should().Be("win1");
        viewModel.SelectedTarget.Should().BeSameAs(selected);
        ph.Should().Equal(new Dictionary<string, int>
        {
            { "ProcessName", 1 },
        });

        viewModel.WindowName = "win2";
        viewModel.ProcessName.Should().Be("test2");
        viewModel.SelectedTarget.ProcessName.Should().Be("test2");
        viewModel.WindowName.Should().Be("win2");
        viewModel.SelectedTarget.WindowName.Should().Be("win2");
        viewModel.SelectedTarget.Should().BeSameAs(selected);
        ph.Should().Equal(new Dictionary<string, int>
        {
            { "ProcessName", 1 },
            { "WindowName", 1 },
        });

        viewModel.SelectedTarget = new ObservableCaptureTarget
        {
            ProcessName = "testNew",
            WindowName = "winNew",
        };
        viewModel.ProcessName.Should().Be("testNew");
        viewModel.SelectedTarget.ProcessName.Should().Be("testNew");
        viewModel.WindowName.Should().Be("winNew");
        viewModel.SelectedTarget.WindowName.Should().Be("winNew");
        viewModel.SelectedTarget.Should().NotBeSameAs(selected);
        ph.Should().Equal(new Dictionary<string, int>
        {
            { "ProcessName", 1 },
            { "WindowName", 1 },
        });
    }

    [Fact]
    public void FilteredWindowProcesses()
    {
        static Mock<IWindowProcessHandle> CreateMock(string process, string window)
        {
            var mock = new Mock<IWindowProcessHandle>();
            mock.SetupGet(x => x.IsActive).Returns(true);
            mock.SetupGet(x => x.ProcessName).Returns(process);
            mock.Setup(x => x.GetCurrentWindowName()).Returns(window);
            return mock;
        }
        var processes = new Mock<IWindowProcessHandle>[]
        {
            CreateMock("Shinkansen", "nozomi"),
            CreateMock("Shinkansen", "kodama"),
            CreateMock("Shinkansen", "sakura"),

            CreateMock("JRkyushu", "tsubame"),
            CreateMock("JRkyushu", "hayabusa"),
        };

        var messenger = new WeakReferenceMessenger();
        var viewModel = new CaptureTargetSelectionWindowViewModel(messenger, processes.Select(m => m.Object).ToArray(), Array.Empty<CaptureTarget>())
        {
            SelectedTarget = new ObservableCaptureTarget
            {
                ProcessName = "Shinkansen",
                WindowName = "",
            },
        };

        viewModel.FilteredWindowProcesses.Should().Equal(new[]
        {
            processes[0].Object,
            processes[1].Object,
            processes[2].Object,
        });


        viewModel.SelectedTarget = new ObservableCaptureTarget
        {
            ProcessName = "Shinkansen2",
            WindowName = "",
        };
        viewModel.FilteredWindowProcesses.Should().BeEmpty();


        viewModel.SelectedTarget = new ObservableCaptureTarget
        {
            ProcessName = "shin",
            WindowName = "a",
        };
        viewModel.FilteredWindowProcesses.Should().Equal(new[]
        {
            processes[1].Object,
            processes[2].Object,
        });

        viewModel.SelectedTarget = new ObservableCaptureTarget
        {
            WindowName = "am",
        };
        viewModel.FilteredWindowProcesses.Should().Equal(new[]
        {
            processes[1].Object,
            processes[3].Object,
        });

        viewModel.SelectedTarget = new ObservableCaptureTarget
        {
            WindowName = "",
            ProcessName = "",
        };
        viewModel.FilteredWindowProcesses.Should().Equal(new[]
        {
            processes[0].Object,
            processes[1].Object,
            processes[2].Object,
            processes[3].Object,
            processes[4].Object,
        });
    }

    [Fact]
    public void ReceiveCurrentWindowProcessHandlesMessage()
    {
        var messenger = new WeakReferenceMessenger();
        var selected = new ObservableCaptureTarget
        {
            ProcessName = "test1",
            WindowName = "win1",
        };
        var processes1 = new IWindowProcessHandle[1];
        var processes2 = new IWindowProcessHandle[1];
        processes1.Should().NotBeSameAs(processes2);

        var viewModel = new CaptureTargetSelectionWindowViewModel(messenger, processes1, Array.Empty<CaptureTarget>())
        {
            SelectedTarget = selected,
        };
        viewModel.WindowProcesses.Should().BeSameAs(processes1);
        messenger.Send(new CurrentWindowProcessHandlesMessage(processes2));
        viewModel.WindowProcesses.Should().BeSameAs(processes2);
    }
}