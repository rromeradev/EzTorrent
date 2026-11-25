using System.Windows;
using System.Windows.Controls;
namespace EzTorrent.Desktop.Controls;
public sealed class CaptionButton : Button
{
    public static readonly DependencyProperty IsNativeHoveredProperty = DependencyProperty.Register(nameof(IsNativeHovered), typeof(bool), typeof(CaptionButton), new PropertyMetadata(false));
    public static readonly DependencyProperty IsNativePressedProperty = DependencyProperty.Register(nameof(IsNativePressed), typeof(bool), typeof(CaptionButton), new PropertyMetadata(false));
    public bool IsNativeHovered { get => (bool)GetValue(IsNativeHoveredProperty); set => SetValue(IsNativeHoveredProperty, value); }
    public bool IsNativePressed { get => (bool)GetValue(IsNativePressedProperty); set => SetValue(IsNativePressedProperty, value); }
}
