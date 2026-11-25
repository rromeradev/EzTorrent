using System.Windows;
using EzTorrent.Core.Settings;

namespace EzTorrent.Desktop.Services;

public sealed class ThemeService(SettingsSession session)
{
    public void Start()
    {
        Apply();
        session.Changed += (_, _) => Apply();
    }

    private void Apply()
    {
        // WPF still marks programmatic ThemeMode access experimental in this SDK.
        #pragma warning disable WPF0001
        Application.Current.ThemeMode = session.Current.Theme switch
        {
            AppTheme.Light => ThemeMode.Light,
            AppTheme.Dark => ThemeMode.Dark,
            _ => ThemeMode.System
        };
        #pragma warning restore WPF0001
    }
}
