using System.ComponentModel;
using System.IO;
using System.Windows;
using EzTorrent.Core.Settings;
using EzTorrent.Desktop.Controls;
using EzTorrent.Desktop.Services;
using EzTorrent.Desktop.ViewModels;
namespace EzTorrent.Desktop.Views;
public partial class MainWindow : AppWindow
{
    private readonly SettingsSession session;
    private readonly UserDialogs dialogs;
    private bool closeApproved;
    private bool closePending;
    public MainWindow(MainViewModel viewModel, SettingsSession session, UserDialogs dialogs)
    {
        this.session = session; this.dialogs = dialogs;
        InitializeComponent(); DataContext = viewModel;
    }
    private void OpenPreferences(object sender, RoutedEventArgs e) => new PreferencesWindow(session) { Owner = this }.ShowDialog();
    private async void OnClosing(object? sender, CancelEventArgs e)
    {
        if (closeApproved) return;
        e.Cancel = true;
        if (closePending) return;
        closePending = true;
        try
        {
            var behavior = session.Current.CloseBehavior;
            if (behavior == CloseBehavior.Ask)
            {
                var selected = dialogs.AskClose(this);
                if (selected is null) return;
                behavior = selected.Value;
                try { await session.UpdateAsync(settings => settings with { CloseBehavior = behavior }); }
                catch (Exception error) when (error is IOException or UnauthorizedAccessException) { dialogs.ShowSaveError(); return; }
            }
            if (behavior == CloseBehavior.Minimize) WindowState = WindowState.Minimized;
            else { closeApproved = true; _ = Dispatcher.BeginInvoke(new Action(Close)); }
        }
        finally { closePending = false; }
    }
}
