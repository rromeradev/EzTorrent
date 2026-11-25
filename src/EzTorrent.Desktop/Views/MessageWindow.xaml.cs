using System.Windows;
using System.Windows.Controls;
using EzTorrent.Desktop.Controls;
namespace EzTorrent.Desktop.Views;
public enum AppDialogResult { Cancel, Accept, Minimize }
public sealed record DialogChoice(AppDialogResult Result, string Label, bool IsDefault = false, bool IsCancel = false);
public partial class MessageWindow : AppWindow
{
    public AppDialogResult SelectedResult { get; private set; } = AppDialogResult.Cancel;
    public MessageWindow(string message, params DialogChoice[] choices)
    {
        InitializeComponent();
        MessageText.Text = message;
        foreach (var choice in choices)
        {
            var button = new Button { Content = choice.Label, MinWidth = 90, Padding = new Thickness(14, 8, 14, 8), Margin = new Thickness(8, 4, 0, 0), IsDefault = choice.IsDefault, IsCancel = choice.IsCancel };
            button.Click += (_, _) => { SelectedResult = choice.Result; DialogResult = true; };
            Choices.Children.Add(button);
        }
    }
}
