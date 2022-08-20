using System.Diagnostics;
using System.Windows.Controls;

namespace Kzrnm.WindowScreenshot.Views;

/// <summary>
/// MainBody.xaml の相互作用ロジック
/// </summary>
public partial class MainBody : UserControl
{
    public MainBody()
    {
        InitializeComponent();
#if DEBUG
        var debugArea = new DebugArea();
        FooterLeft.Children.Insert(0, debugArea);
#endif
    }
}
