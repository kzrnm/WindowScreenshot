using CommunityToolkit.Mvvm.DependencyInjection;
using Kzrnm.WindowScreenshot.Image.Capture;
using Kzrnm.WindowScreenshot.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace Kzrnm.WindowScreenshot.Views;
/// <summary>
/// CaptureTargetSelectionWindow.xaml の相互作用ロジック
/// </summary>
public partial class CaptureTargetSelectionWindow : Window
{
    public CaptureTargetSelectionWindow(IList<CaptureTarget> collection)
    {
        InitializeComponent();
        var factory = Ioc.Default.GetRequiredServiceIfIsNotInDesignMode<CaptureTargetSelectionWindowViewModel.Factory>(this);
        DataContext = factory.Build(collection);
    }
    public IList<CaptureTarget>? ShowDialogWithResponse()
    {
        if (ShowDialog() != true)
            return null;

        return (DataContext as CaptureTargetSelectionWindowViewModel)?.GetCaptureTargets().ToArray();
    }

    protected override void OnClosing(CancelEventArgs e)
    {
        if (DialogResult == null && (DataContext as CaptureTargetSelectionWindowViewModel)?.IsUpdated is true)
        {
            var result = MessageBox.Show(this, Properties.Resources.SaveChangesMessage, AppDomain.CurrentDomain.FriendlyName, MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    DialogResult = true;
                    break;
                case MessageBoxResult.No:
                    DialogResult = false;
                    break;
                case MessageBoxResult.Cancel:
                    e.Cancel = true;
                    return;
            }
        }
    }
}