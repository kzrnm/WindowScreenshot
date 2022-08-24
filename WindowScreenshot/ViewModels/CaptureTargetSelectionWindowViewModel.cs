using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Kzrnm.WindowScreenshot.Image.Capture;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kzrnm.WindowScreenshot.ViewModels;
public partial class CaptureTargetSelectionWindowViewModel : ObservableRecipient, IRecipient<CurrentWindowProcessHandlesMessage>
{
    public record Factory(IMessenger Messenger, IObserveWindowProcess ObserveWindowProcess)
    {
        public Factory(IObserveWindowProcess observeWindowProcess)
            : this(WeakReferenceMessenger.Default, observeWindowProcess) { }

        public CaptureTargetSelectionWindowViewModel Build(IEnumerable<CaptureTarget> targets)
        {
            return new(Messenger, ObserveWindowProcess.CurrentWindows.ToArray(), targets);
        }
    }

    public CaptureTargetSelectionWindowViewModel(IMessenger messenger, IEnumerable<IWindowProcessHandle> windowProcesses, IEnumerable<CaptureTarget> targets)
        : base(messenger)
    {
        _WindowProcesses = windowProcesses;
        CaptureTargetWindows = new(targets.Select(c => new ObservableCaptureTarget(c)));
        if (CaptureTargetWindows.Count > 0)
            CaptureTargetWindows.SelectedIndex = 0;
        CaptureTargetWindows.CollectionChanged += UpdatedHandler;
        IsActive = true;
    }

    public SelectorObservableCollection<ObservableCaptureTarget> CaptureTargetWindows { get; }
    public IEnumerable<CaptureTarget> GetCaptureTargets()
        => CaptureTargetWindows.Select(c => c.ToCaptureTarget());

    public bool IsUpdated { get; private set; }
    private void UpdatedHandler(object? sender, EventArgs e)
    {
        IsUpdated = true;
    }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(FilteredWindowProcesses))]
    private ObservableCaptureTarget? _SelectedTarget;
    partial void OnSelectedTargetChanging(ObservableCaptureTarget? value)
    {
        if (_SelectedTarget is { } selected)
            selected.PropertyChanged -= UpdatedHandler;
        if (value is { } next)
            next.PropertyChanged += UpdatedHandler;
    }
    partial void OnSelectedTargetChanged(ObservableCaptureTarget? value)
    {
        if (value is { } selected)
        {
            ProcessName = selected.ProcessName;
            WindowName = selected.WindowName;
        }
    }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(FilteredWindowProcesses))]
    private string _ProcessName = "";
    partial void OnProcessNameChanged(string value)
    {
        if (SelectedTarget is { } selected)
            selected.ProcessName = value;
    }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(FilteredWindowProcesses))]
    private string _WindowName = "";
    partial void OnWindowNameChanged(string value)
    {
        if (SelectedTarget is { } selected)
            selected.WindowName = value;
    }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(FilteredWindowProcesses))]
    private IEnumerable<IWindowProcessHandle> _WindowProcesses;

    public IEnumerable<IWindowProcessHandle>? FilteredWindowProcesses
        => WindowProcesses?.Where(w => SelectedTarget?.IsFitFor(w) ?? false);

    [RelayCommand]
    public void AddCaptureWindow()
    {
        CaptureTargetWindows.Add(new());
    }

    [RelayCommand]
    public void RemoveSelectedCaptureTarget()
    {
        CaptureTargetWindows.RemoveSelectedItem();
    }
    public void Receive(CurrentWindowProcessHandlesMessage message)
    {
        WindowProcesses = message.Value;
    }

    public partial class ObservableCaptureTarget : ObservableObject
    {
        public ObservableCaptureTarget() { }
        public ObservableCaptureTarget(CaptureTarget original)
        {
            _ProcessName = original.ProcessName;
            _WindowName = original.WindowName;
            _OnlyTargetWindow = original.OnlyTargetWindow;

            _RegionLeft = original.Region.Left;
            _RegionRight = original.Region.Right;
            _RegionTop = original.Region.Top;
            _RegionBottom = original.Region.Bottom;
            _RegionWidth = original.Region.Width;
            _RegionHeight = original.Region.Height;
            _RegionUseRect = original.Region.UseRect;
        }

        [ObservableProperty]
        private string _ProcessName = "";
        [ObservableProperty]
        private string _WindowName = "";
        [ObservableProperty]
        private bool _OnlyTargetWindow;

        [ObservableProperty]
        private int _RegionLeft;

        [ObservableProperty]
        private int _RegionRight;

        [ObservableProperty]
        private int _RegionTop;

        [ObservableProperty]
        private int _RegionBottom;

        [ObservableProperty]
        private int _RegionWidth;

        [ObservableProperty]
        private int _RegionHeight;

        [ObservableProperty]
        private bool _RegionUseRect;

        public CaptureTarget ToCaptureTarget()
            => new(ProcessName,
                WindowName,
                new(
                    RegionLeft,
                    RegionRight,
                    RegionTop,
                    RegionBottom,
                    RegionWidth,
                    RegionHeight,
                    RegionUseRect
                ),
                OnlyTargetWindow);

        public string DisplayText
        {
            get
            {
                if (string.IsNullOrEmpty(WindowName))
                    if (string.IsNullOrEmpty(ProcessName)) return "{none}";
                    else return ProcessName;
                return $"{ProcessName}[{WindowName}]";
            }
        }

        public bool IsFitFor(IWindowProcessHandle window)
            => CaptureTarget.IsFitFor(window, ProcessName, WindowName);
    }
}
