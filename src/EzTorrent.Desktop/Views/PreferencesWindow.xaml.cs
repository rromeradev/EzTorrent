using System.IO;
using System.Windows;
using EzTorrent.Core.Settings;
using EzTorrent.Desktop.Localization;
using EzTorrent.Desktop.Services;
using EzTorrent.Desktop.Controls;

namespace EzTorrent.Desktop.Views;

public partial class PreferencesWindow : AppWindow
{
    private readonly SettingsSession session;
    private readonly bool isSetup;
    private bool saving;

    public PreferencesWindow(SettingsSession session, bool isSetup = false)
    {
        this.session = session;
        this.isSetup = isSetup;
        InitializeComponent();
        ThemeBox.SelectedValue = session.Current.Theme.ToString();
        LanguageBox.SelectedValue = session.Current.Language ?? "";
        CloseBox.SelectedValue = session.Current.CloseBehavior.ToString();
        FolderBox.Text = session.Current.DownloadDirectory;
        if (isSetup)
        {
            Title = Heading.Text = Texts.Get("Welcome");
            Introduction.Visibility = Visibility.Visible;
            SaveButton.Content = Texts.Get("GetStarted");
        }
        Closing += (_, e) => { if (saving) e.Cancel = true; };
    }

    private void BrowseFolder(object sender, RoutedEventArgs e)
    {
        var dialog = new FolderPickerWindow(FolderBox.Text) { Owner = this };
        if (dialog.ShowDialog() == true) FolderBox.Text = dialog.SelectedPath;
    }

    private async void SavePreferences(object sender, RoutedEventArgs e)
    {
        var folder = FolderBox.Text.Trim();
        if (!SettingsPolicy.IsValidDownloadDirectory(folder))
        {
            ErrorText.Text = Texts.Get("InvalidFolder");
            return;
        }
        var theme = Enum.Parse<AppTheme>((string)ThemeBox.SelectedValue);
        var close = Enum.Parse<CloseBehavior>((string)CloseBox.SelectedValue);
        var language = (string)LanguageBox.SelectedValue;
        saving = true;
        SaveButton.IsEnabled = CancelButton.IsEnabled = false;
        try
        {
            await session.UpdateAsync(settings => settings with
            {
                Theme = theme, CloseBehavior = close, Language = language.Length == 0 ? null : language,
                DownloadDirectory = folder, HasCompletedSetup = isSetup || settings.HasCompletedSetup
            });
            saving = false;
            DialogResult = true;
        }
        catch (Exception error) when (error is IOException or UnauthorizedAccessException)
        {
            ErrorText.Text = Texts.Get("SaveError");
        }
        finally { saving = false; SaveButton.IsEnabled = CancelButton.IsEnabled = true; }
    }
}
