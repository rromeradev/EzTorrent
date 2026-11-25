using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using EzTorrent.Desktop.Controls;
namespace EzTorrent.Desktop.Behaviors;
internal static class NativeCaption
{
    public static void Attach(AppWindow window)
    {
        var source = HwndSource.FromHwnd(new WindowInteropHelper(window).Handle);
        var disabled = 1; // DWMNCRP_DISABLED: app draws the only visible nonclient frame.
        _ = DwmSetWindowAttribute(source.Handle, 2, ref disabled, sizeof(int));
        var supportsSnap = OperatingSystem.IsWindowsVersionAtLeast(10, 0, 22000);
        if (supportsSnap)
        {
            var noBorder = unchecked((int)0xfffffffe); // DWMWA_COLOR_NONE
            _ = DwmSetWindowAttribute(source.Handle, 34, ref noBorder, sizeof(int));
        }
        var pressed = false;
        bool Contains(Point screen) => window.MaximizeButton is { IsVisible: true, IsEnabled: true } button && window.ShowMaximize && new Rect(button.RenderSize).Contains(button.PointFromScreen(screen));
        void Hover(bool value) { if (window.MaximizeButton is { } button) button.IsNativeHovered = value; }
        void Reset()
        {
            pressed = false;
            if (window.MaximizeButton is { } button) { button.IsNativePressed = false; button.IsNativeHovered = false; }
        }
        HwndSourceHook hook = (IntPtr hwnd, int message, IntPtr wParam, IntPtr lParam, ref bool handled) =>
        {
            switch (message)
            {
                case 0x0024: // WM_GETMINMAXINFO: frameless windows must respect the current monitor work area.
                    var monitor = new MonitorInfo { Size = Marshal.SizeOf<MonitorInfo>() };
                    if (GetMonitorInfo(MonitorFromWindow(hwnd, 2), ref monitor))
                    {
                        var limits = Marshal.PtrToStructure<MinMaxInfo>(lParam);
                        limits.MaxPosition = new NativePoint(monitor.Work.Left - monitor.Bounds.Left, monitor.Work.Top - monitor.Bounds.Top);
                        limits.MaxSize = new NativePoint(monitor.Work.Right - monitor.Work.Left, monitor.Work.Bottom - monitor.Work.Top);
                        var scale = source.CompositionTarget.TransformToDevice;
                        limits.MinTrackSize = new NativePoint(Math.Max(limits.MinTrackSize.X, (int)Math.Ceiling(window.MinWidth * scale.M11)), Math.Max(limits.MinTrackSize.Y, (int)Math.Ceiling(window.MinHeight * scale.M22)));
                        Marshal.StructureToPtr(limits, lParam, false);
                        handled = true;
                    }
                    break;
                case 0x0084 when supportsSnap: // WM_NCHITTEST: live geometry, never a cached screen rectangle.
                    if (Contains(Unpack(lParam))) { handled = true; return new IntPtr(9); }
                    break;
                case 0x00A0: // WM_NCMOUSEMOVE
                    Hover(Contains(Unpack(lParam)));
                    var tracking = new MouseTracking { Size = Marshal.SizeOf<MouseTracking>(), Flags = 0x12, Window = hwnd };
                    _ = TrackMouseEvent(ref tracking);
                    break; // Keep OS Snap Layout hover behavior.
                case 0x02A2: Hover(false); break; // WM_NCMOUSELEAVE
                case 0x00A1 when wParam.ToInt32() == 9: // WM_NCLBUTTONDOWN / HTMAXBUTTON
                    pressed = true;
                    if (window.MaximizeButton is { } down) down.IsNativePressed = true;
                    _ = SetCapture(hwnd); handled = true; break;
                case 0x0200 when pressed: // Captured WM_MOUSEMOVE has client-pixel coordinates.
                    var client = source.CompositionTarget.TransformFromDevice.Transform(Unpack(lParam));
                    Hover(Contains(window.PointToScreen(client)));
                    if (window.MaximizeButton is { } moving) moving.IsNativePressed = moving.IsNativeHovered;
                    handled = true; break;
                case 0x0202 when pressed: // WM_LBUTTONUP; leaving the button cancels the action.
                    var point = source.CompositionTarget.TransformFromDevice.Transform(Unpack(lParam));
                    var activate = Contains(window.PointToScreen(point));
                    Reset(); _ = ReleaseCapture();
                    if (activate) _ = window.Dispatcher.BeginInvoke(new Action(window.ToggleMaximize));
                    handled = true; break;
                case 0x0215: // WM_CAPTURECHANGED
                case 0x001F: // WM_CANCELMODE
                case 0x0006 when (wParam.ToInt64() & 0xffff) == 0:
                    Reset(); break;
            }
            return IntPtr.Zero;
        };
        source.AddHook(hook);
        window.LocationChanged += (_, _) => Hover(false);
        window.SizeChanged += (_, _) => Hover(false);
        window.Closed += (_, _) => source.RemoveHook(hook);
    }
    private static Point Unpack(IntPtr value) => new(unchecked((short)(value.ToInt64() & 0xffff)), unchecked((short)((value.ToInt64() >> 16) & 0xffff)));
    [StructLayout(LayoutKind.Sequential)]
    private struct MouseTracking { public int Size; public int Flags; public IntPtr Window; public int HoverTime; }
    [StructLayout(LayoutKind.Sequential)]
    private readonly record struct NativePoint(int X, int Y);
    [StructLayout(LayoutKind.Sequential)]
    private struct NativeRect { public int Left; public int Top; public int Right; public int Bottom; }
    [StructLayout(LayoutKind.Sequential)]
    private struct MonitorInfo { public int Size; public NativeRect Bounds; public NativeRect Work; public int Flags; }
    [StructLayout(LayoutKind.Sequential)]
    private struct MinMaxInfo { public NativePoint Reserved; public NativePoint MaxSize; public NativePoint MaxPosition; public NativePoint MinTrackSize; public NativePoint MaxTrackSize; }
    [DllImport("user32.dll"), DefaultDllImportSearchPaths(DllImportSearchPath.System32)] private static extern IntPtr SetCapture(IntPtr window);
    [DllImport("user32.dll"), DefaultDllImportSearchPaths(DllImportSearchPath.System32)] [return: MarshalAs(UnmanagedType.Bool)] private static extern bool ReleaseCapture();
    [DllImport("user32.dll"), DefaultDllImportSearchPaths(DllImportSearchPath.System32)] [return: MarshalAs(UnmanagedType.Bool)] private static extern bool TrackMouseEvent(ref MouseTracking tracking);
    [DllImport("dwmapi.dll"), DefaultDllImportSearchPaths(DllImportSearchPath.System32)] private static extern int DwmSetWindowAttribute(IntPtr window, int attribute, ref int value, int size);
    [DllImport("user32.dll"), DefaultDllImportSearchPaths(DllImportSearchPath.System32)] private static extern IntPtr MonitorFromWindow(IntPtr window, int flags);
    [DllImport("user32.dll", EntryPoint = "GetMonitorInfoW"), DefaultDllImportSearchPaths(DllImportSearchPath.System32)] [return: MarshalAs(UnmanagedType.Bool)] private static extern bool GetMonitorInfo(IntPtr monitor, ref MonitorInfo info);
}
