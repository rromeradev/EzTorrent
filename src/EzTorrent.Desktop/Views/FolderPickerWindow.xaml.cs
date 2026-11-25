using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using EzTorrent.Core.Settings;
using EzTorrent.Desktop.Controls;
using EzTorrent.Desktop.Localization;
namespace EzTorrent.Desktop.Views;
public sealed record FolderEntry(string Name, string Path);
public partial class FolderPickerWindow : AppWindow
{
    private CancellationTokenSource? scanCancellation;
    private string currentPath = "";
    private bool closed;
    public string SelectedPath { get; private set; } = "";
    public FolderPickerWindow(string initialPath)
    {
        InitializeComponent();
        var initial = Directory.Exists(initialPath) ? initialPath : Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        Loaded += async (_, _) => await NavigateAsync(initial);
        Closed += (_, _) => { closed = true; scanCancellation?.Cancel(); };
    }
    private async Task NavigateAsync(string path)
    {
        if (!SettingsPolicy.IsValidDownloadDirectory(path) || !Directory.Exists(path)) { ErrorText.Text = Texts.Get("FolderReadError"); return; }
        using var cancellation = new CancellationTokenSource();
        scanCancellation = cancellation;
        Navigation.IsEnabled = Folders.IsEnabled = SelectButton.IsEnabled = false;
        ErrorText.Text = Texts.Get("LoadingFolders");
        try
        {
            var entries = await Task.Run(() => Directory.EnumerateDirectories(path).Select(item =>
            {
                cancellation.Token.ThrowIfCancellationRequested();
                return new FolderEntry(System.IO.Path.GetFileName(item), item);
            }).OrderBy(item => item.Name, StringComparer.CurrentCultureIgnoreCase).ToArray(), cancellation.Token);
            if (closed) return;
            currentPath = System.IO.Path.GetFullPath(path);
            LocationBox.Text = currentPath;
            Folders.ItemsSource = entries;
            ErrorText.Text = "";
        }
        catch (OperationCanceledException) { }
        catch (Exception error) when (error is IOException or UnauthorizedAccessException or ArgumentException or System.Security.SecurityException)
        {
            if (!closed) ErrorText.Text = Texts.Get("FolderReadError");
        }
        finally
        {
            scanCancellation = null;
            if (!closed) Navigation.IsEnabled = Folders.IsEnabled = SelectButton.IsEnabled = true;
        }
    }
    private void ShowDrives(object sender, RoutedEventArgs e)
    {
        currentPath = LocationBox.Text = "";
        Folders.ItemsSource = DriveInfo.GetDrives().Select(drive => new FolderEntry(drive.Name, drive.Name)).ToArray();
        ErrorText.Text = "";
    }
    private async void GoUp(object sender, RoutedEventArgs e)
    {
        if (currentPath.Length == 0) { ShowDrives(sender, e); return; }
        var parent = Directory.GetParent(currentPath);
        if (parent is null) ShowDrives(sender, e); else await NavigateAsync(parent.FullName);
    }
    private async void EnterFolder(object sender, MouseButtonEventArgs e)
    {
        if (Folders.SelectedItem is FolderEntry entry) await NavigateAsync(entry.Path);
    }
    private async void FolderKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter && Folders.SelectedItem is FolderEntry entry) { e.Handled = true; await NavigateAsync(entry.Path); }
    }
    private async void LocationKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter) { e.Handled = true; await NavigateAsync(LocationBox.Text.Trim()); }
    }
    private void LocationTextChanged(object sender, TextChangedEventArgs e)
    {
        // A typed path is a new selection intent, including failed navigation attempts.
        if (Folders is not null) Folders.SelectedItem = null;
    }
    private void SelectFolder(object sender, RoutedEventArgs e)
    {
        var path = Folders.SelectedItem is FolderEntry entry ? entry.Path : LocationBox.Text.Trim();
        if (!SettingsPolicy.IsValidDownloadDirectory(path) || !Directory.Exists(path)) { ErrorText.Text = Texts.Get("FolderReadError"); return; }
        SelectedPath = System.IO.Path.GetFullPath(path);
        DialogResult = true;
    }
}
