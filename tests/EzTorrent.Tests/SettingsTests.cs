using System.Globalization;
using System.Text.Json;
using EzTorrent.Core.Settings;
using EzTorrent.Infrastructure.Settings;
using Xunit;

namespace EzTorrent.Tests;

public sealed class SettingsTests : IDisposable
{
    private readonly string directory = Path.Combine(Path.GetTempPath(), "EzTorrent.Tests", Guid.NewGuid().ToString("N"));
    private string SettingsPath => Path.Combine(directory, "settings.json");
    private JsonSettingsStore Store() => new(SettingsPath);

    [Fact]
    public async Task MissingFileReturnsPrivateFirstRunDefaults()
    {
        var settings = await Store().LoadAsync(TestContext.Current.CancellationToken);
        Assert.False(settings.TelemetryEnabled);
        Assert.False(settings.HasCompletedSetup);
        Assert.Equal(AppTheme.System, settings.Theme);
        Assert.Equal(CloseBehavior.Ask, settings.CloseBehavior);
        Assert.False(File.Exists(SettingsPath));
    }

    [Theory]
    [InlineData("pt-BR", "pt-BR")]
    [InlineData("pt-PT", "pt-BR")]
    [InlineData("en-GB", "en-US")]
    [InlineData("ja-JP", "en-US")]
    public void WindowsLanguageHasDeterministicFallback(string input, string expected)
        => Assert.Equal(expected, SettingsPolicy.ResolveLanguage(null, CultureInfo.GetCultureInfo(input)));

    [Fact]
    public void ExplicitLanguageOverridesWindows()
        => Assert.Equal("en-US", SettingsPolicy.ResolveLanguage("en-US", CultureInfo.GetCultureInfo("pt-BR")));

    [Fact]
    public async Task SaveRoundTripsAndAtomicallyReplacesExistingDocument()
    {
        var store = Store();
        await store.SaveAsync(new AppSettings { Theme = AppTheme.Light }, TestContext.Current.CancellationToken);
        var expected = new AppSettings { Theme = AppTheme.Dark, Language = "pt-BR", HasCompletedSetup = true };
        await store.SaveAsync(expected, TestContext.Current.CancellationToken);
        Assert.Equal(expected, await store.LoadAsync(TestContext.Current.CancellationToken));
        Assert.Single(Directory.GetFiles(directory));
    }

    [Theory]
    [InlineData("{ broken")]
    [InlineData("null")]
    [InlineData("{\"schemaVersion\":999}")]
    [InlineData("{\"theme\":999}")]
    [InlineData("{\"language\":\"xx\"}")]
    public async Task InvalidDocumentsAreReportedAndPreserved(string json)
    {
        Directory.CreateDirectory(directory);
        await File.WriteAllTextAsync(SettingsPath, json, TestContext.Current.CancellationToken);
        await Assert.ThrowsAsync<SettingsFormatException>(() => Store().LoadAsync(TestContext.Current.CancellationToken));
        Assert.Equal(json, await File.ReadAllTextAsync(SettingsPath, TestContext.Current.CancellationToken));
    }

    [Fact]
    public async Task ConcurrentWritesProduceOneCompleteDocument()
    {
        var store = Store();
        await Task.WhenAll(Enumerable.Range(0, 20).Select(i => store.SaveAsync(new AppSettings { Theme = i % 2 == 0 ? AppTheme.Dark : AppTheme.Light }, TestContext.Current.CancellationToken)));
        var loaded = await store.LoadAsync(TestContext.Current.CancellationToken);
        Assert.Contains(loaded.Theme, new[] { AppTheme.Dark, AppTheme.Light });
        Assert.Single(Directory.GetFiles(directory));
    }

    [Fact]
    public async Task CancelledSavePreservesPreviousSettings()
    {
        var store = Store();
        await store.SaveAsync(new AppSettings { Theme = AppTheme.Dark }, TestContext.Current.CancellationToken);
        using var cancellation = new CancellationTokenSource();
        cancellation.Cancel();
        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => store.SaveAsync(new AppSettings { Theme = AppTheme.Light }, cancellation.Token));
        Assert.Equal(AppTheme.Dark, (await store.LoadAsync(TestContext.Current.CancellationToken)).Theme);
        Assert.Single(Directory.GetFiles(directory));
    }

    [Theory]
    [InlineData("C:\\bad\0folder")]
    [InlineData("C:\\bad<folder")]
    public async Task InvalidAbsolutePathsAreRejectedOnLoadAndPreserved(string folder)
    {
        Directory.CreateDirectory(directory);
        var json = JsonSerializer.Serialize(new { schemaVersion = 1, downloadDirectory = folder });
        await File.WriteAllTextAsync(SettingsPath, json, TestContext.Current.CancellationToken);
        await Assert.ThrowsAsync<SettingsFormatException>(() => Store().LoadAsync(TestContext.Current.CancellationToken));
        Assert.Equal(json, await File.ReadAllTextAsync(SettingsPath, TestContext.Current.CancellationToken));
    }

    public void Dispose()
    {
        if (Directory.Exists(directory)) Directory.Delete(directory, recursive: true);
    }

    [Theory]
    [InlineData("Grid")]
    [InlineData("Cards")]
    public async Task LegacyLayoutIsDiscardedWithoutLosingOtherPreferences(string layout)
    {
        Directory.CreateDirectory(directory);
        var json = JsonSerializer.Serialize(new { schemaVersion = 1, layout, theme = "Dark", language = "pt-BR", hasCompletedSetup = true });
        await File.WriteAllTextAsync(SettingsPath, json, TestContext.Current.CancellationToken);
        var store = Store();
        var settings = await store.LoadAsync(TestContext.Current.CancellationToken);
        Assert.Equal(AppTheme.Dark, settings.Theme);
        Assert.Equal("pt-BR", settings.Language);
        Assert.True(settings.HasCompletedSetup);
        await store.SaveAsync(settings, TestContext.Current.CancellationToken);
        using var saved = JsonDocument.Parse(await File.ReadAllTextAsync(SettingsPath, TestContext.Current.CancellationToken));
        Assert.False(saved.RootElement.TryGetProperty("layout", out _));
    }
}
