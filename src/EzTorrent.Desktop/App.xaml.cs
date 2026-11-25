using System.IO;
using System.Globalization;
using System.Windows;
using EzTorrent.Core.Settings;
using EzTorrent.Desktop.Localization;
using EzTorrent.Desktop.Services;
using EzTorrent.Desktop.ViewModels;
using EzTorrent.Desktop.Views;
using EzTorrent.Infrastructure.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EzTorrent.Desktop;

public partial class App : Application
{
    private static readonly CultureInfo WindowsUiCulture = CultureInfo.CurrentUICulture;
    private IHost? host;
    private Mutex? instanceMutex;
    private bool ownsMutex;

    protected override async void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        SetLanguage(null);
        instanceMutex = new Mutex(true, @"Local\EzTorrent.Desktop.Foundation", out ownsMutex);
        if (!ownsMutex)
        {
            UserDialogs.ShowMessage(Texts.Get("AlreadyRunning"));
            Shutdown();
            return;
        }
        var dataDirectory = Environment.GetEnvironmentVariable("EZTORRENT_DATA_DIRECTORY")
            ?? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "EzTorrent");
        var settingsPath = Path.Combine(dataDirectory, "settings.json");
        try
        {
            var builder = Host.CreateApplicationBuilder(new HostApplicationBuilderSettings
            {
                Args = [], DisableDefaults = true, ContentRootPath = AppContext.BaseDirectory
            });
            builder.Logging.ClearProviders();
            builder.Services.AddSingleton<ISettingsStore>(new JsonSettingsStore(settingsPath));
            builder.Services.AddSingleton<SettingsSession>();
            builder.Services.AddSingleton<ThemeService>();
            builder.Services.AddSingleton<UserDialogs>();
            builder.Services.AddSingleton<MainViewModel>();
            builder.Services.AddSingleton<MainWindow>();
            host = builder.Build();
            var session = host.Services.GetRequiredService<SettingsSession>();
            await session.InitializeAsync();
            SetLanguage(session.Current.Language);
            host.Services.GetRequiredService<ThemeService>().Start();
            await host.StartAsync();
            MainWindow = host.Services.GetRequiredService<MainWindow>();
            ShutdownMode = ShutdownMode.OnMainWindowClose;
            MainWindow.Show();
            if (!session.Current.HasCompletedSetup)
                new PreferencesWindow(session, isSetup: true) { Owner = MainWindow }.ShowDialog();
        }
        catch (Exception error) when (error is IOException or UnauthorizedAccessException or SettingsFormatException)
        {
            UserDialogs.ShowMessage($"{Texts.Get("LoadError")}\n{settingsPath}");
            Shutdown(1);
        }
    }

    private static void SetLanguage(string? preference)
    {
        var culture = CultureInfo.GetCultureInfo(SettingsPolicy.ResolveLanguage(preference, WindowsUiCulture));
        Texts.UseCulture(culture);
        CultureInfo.DefaultThreadCurrentUICulture = culture;
        CultureInfo.CurrentUICulture = culture;
    }

    protected override void OnExit(ExitEventArgs e)
    {
        // No background work exists in Foundation. Future engine shutdown must become async before exiting.
        host?.Dispose();
        if (ownsMutex) instanceMutex?.ReleaseMutex();
        instanceMutex?.Dispose();
        base.OnExit(e);
    }
}
