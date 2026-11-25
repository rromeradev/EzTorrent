using System.Windows.Controls;
using EzTorrent.Desktop;
using EzTorrent.Desktop.Views;
using Xunit;

namespace EzTorrent.Tests;

public sealed class FolderPickerTests
{
    [Fact]
    public async Task EditingLocationDiscardsThePreviouslySelectedFolder()
    {
        var completion = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var thread = new Thread(() =>
        {
            try
            {
                var app = new App();
                app.InitializeComponent();
                var picker = new FolderPickerWindow(Path.GetTempPath());
                var folders = (ListBox)picker.FindName("Folders");
                var location = (TextBox)picker.FindName("LocationBox");
                folders.ItemsSource = new[] { new FolderEntry("Previous selection", Path.GetTempPath()) };
                folders.SelectedIndex = 0;
                Assert.NotNull(folders.SelectedItem);
                location.Text = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                Assert.Null(folders.SelectedItem);
                picker.Close();
                app.Shutdown();
                completion.SetResult();
            }
            catch (Exception error) { completion.SetException(error); }
        }) { IsBackground = true };
        thread.SetApartmentState(ApartmentState.STA);
        thread.Start();
        await completion.Task.WaitAsync(TimeSpan.FromSeconds(15), TestContext.Current.CancellationToken);
    }
}
