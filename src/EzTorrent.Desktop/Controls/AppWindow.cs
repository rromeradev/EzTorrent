using System.Windows;
using System.Windows.Shell;
using EzTorrent.Desktop.Behaviors;
namespace EzTorrent.Desktop.Controls;
/// <summary>All app-owned windows share one frame and one set of caption controls.</summary>
public class AppWindow : Window
{
    public static readonly DependencyProperty ShowMinimizeProperty = DependencyProperty.Register(nameof(ShowMinimize), typeof(bool), typeof(AppWindow), new PropertyMetadata(false));
    public static readonly DependencyProperty ShowMaximizeProperty = DependencyProperty.Register(nameof(ShowMaximize), typeof(bool), typeof(AppWindow), new PropertyMetadata(false));
    public bool ShowMinimize { get => (bool)GetValue(ShowMinimizeProperty); set => SetValue(ShowMinimizeProperty, value); }
    public bool ShowMaximize { get => (bool)GetValue(ShowMaximizeProperty); set => SetValue(ShowMaximizeProperty, value); }
    internal CaptionButton? MaximizeButton { get; private set; }
    public AppWindow()
    {
        WindowStyle = WindowStyle.None;
        SetResourceReference(StyleProperty, typeof(AppWindow));
        WindowChrome.SetWindowChrome(this, new WindowChrome { CaptionHeight = 48, ResizeBorderThickness = new Thickness(8), GlassFrameThickness = new Thickness(0), UseAeroCaptionButtons = false });
        SourceInitialized += (_, _) => NativeCaption.Attach(this);
    }
    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();
        MaximizeButton = GetTemplateChild("PART_Maximize") as CaptionButton;
        if (GetTemplateChild("PART_Minimize") is CaptionButton minimize) minimize.Click += (_, _) => SystemCommands.MinimizeWindow(this);
        if (MaximizeButton is { } maximize) maximize.Click += (_, _) => ToggleMaximize();
        if (GetTemplateChild("PART_Close") is CaptionButton close) close.Click += (_, _) => SystemCommands.CloseWindow(this);
    }
    internal void ToggleMaximize()
    {
        if (!ShowMaximize || ResizeMode is ResizeMode.NoResize or ResizeMode.CanMinimize) return;
        if (WindowState == WindowState.Maximized) SystemCommands.RestoreWindow(this);
        else SystemCommands.MaximizeWindow(this);
    }
}
