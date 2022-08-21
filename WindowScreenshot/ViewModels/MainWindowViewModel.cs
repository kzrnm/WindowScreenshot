using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Kzrnm.WindowScreenshot.Image;
using Kzrnm.WindowScreenshot.Properties.App;
using System.Windows.Input;

namespace Kzrnm.WindowScreenshot.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    public MainWindowViewModel(ImageProvider imageProvider)
    {
        ImageProvider = imageProvider;
    }
    public ImageProvider ImageProvider { get; }
    public string Title { get; } = Resources.MainWindowTitle;

    [RelayCommand]
    private void OnKeyDown(KeyEventArgs e)
    {
        switch (e.Key)
        {
            case Key.Escape:
                ImageProvider.Images.Clear();
                break;
        }
    }
}
