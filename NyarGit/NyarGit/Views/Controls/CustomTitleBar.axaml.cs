using System.Reactive;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.VisualTree;
using ReactiveUI;

namespace NyarGit.Views.Controls;
public partial class CustomTitleBar : UserControl
{
    public ReactiveCommand<Unit, Unit> MinimizeCommand { get; }

    public ReactiveCommand<Unit, Unit> MaximizeRestoreCommand { get; }
    public ReactiveCommand<Unit, Unit> CloseCommand { get; }

    public CustomTitleBar()
    {
        InitializeComponent();
        MinimizeCommand = ReactiveCommand.Create(ExecuteMinimize);
        MaximizeRestoreCommand = ReactiveCommand.Create(ExecuteMaximize);
        CloseCommand = ReactiveCommand.Create(ExecuteClose);

        DataContext = this;
    }

    public void ExecuteMinimize()
    {
        if (this.GetVisualRoot() is Window window)
        {
            window.WindowState = WindowState.Minimized;
        }
    }

    public void ExecuteMaximize() 
    {
        if (this.GetVisualRoot() is Window window)
        {
            window.WindowState = window.WindowState == WindowState.Maximized
                ? WindowState.Normal
                : WindowState.Maximized;
        }
    }

    public void ExecuteClose()
    {
        if (this.GetVisualRoot() is Window window)
        {
            window.Close();
        }
    }
}