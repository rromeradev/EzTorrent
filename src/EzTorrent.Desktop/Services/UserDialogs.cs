using System.Windows;
using EzTorrent.Core.Settings;
using EzTorrent.Desktop.Localization;
using EzTorrent.Desktop.Views;
namespace EzTorrent.Desktop.Services;
public sealed class UserDialogs
{
    public void ShowSaveError() => ShowMessage(Texts.Get("SaveError"), Application.Current.MainWindow);
    public static void ShowMessage(string message, Window? owner = null)
    {
        var dialog = new MessageWindow(message, new DialogChoice(AppDialogResult.Accept, Texts.Get("Okay"), IsDefault: true, IsCancel: true));
        SetOwner(dialog, owner);
        dialog.ShowDialog();
    }
    public CloseBehavior? AskClose(Window owner)
    {
        var dialog = new MessageWindow(Texts.Get("ClosePrompt"),
            new DialogChoice(AppDialogResult.Accept, Texts.Get("Exit")),
            new DialogChoice(AppDialogResult.Minimize, Texts.Get("Minimize")),
            new DialogChoice(AppDialogResult.Cancel, Texts.Get("Cancel"), IsDefault: true, IsCancel: true));
        SetOwner(dialog, owner);
        dialog.ShowDialog();
        return dialog.SelectedResult switch { AppDialogResult.Accept => CloseBehavior.Exit, AppDialogResult.Minimize => CloseBehavior.Minimize, _ => null };
    }
    private static void SetOwner(Window dialog, Window? owner)
    {
        if (owner is { IsVisible: true }) dialog.Owner = owner;
        else dialog.WindowStartupLocation = WindowStartupLocation.CenterScreen;
    }
}
