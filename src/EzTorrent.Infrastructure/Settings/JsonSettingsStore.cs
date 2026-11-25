using System.Text.Json;
using System.Text.Json.Serialization;
using EzTorrent.Core.Settings;

namespace EzTorrent.Infrastructure.Settings;

/// <summary>Serializes writes within this instance; the desktop owns a single store and process mutex.</summary>
public sealed class JsonSettingsStore(string path) : ISettingsStore
{
    private static readonly JsonSerializerOptions Options = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true,
        Converters = { new JsonStringEnumConverter() }
    };
    private readonly string fullPath = Path.GetFullPath(path);
    private readonly SemaphoreSlim gate = new(1, 1);

    public async Task<AppSettings> LoadAsync(CancellationToken cancellationToken = default)
    {
        await gate.WaitAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            if (!File.Exists(fullPath)) return new AppSettings();
            await using var stream = new FileStream(fullPath, FileMode.Open, FileAccess.Read,
                FileShare.Read | FileShare.Delete, 4096, FileOptions.Asynchronous);
            var settings = await JsonSerializer.DeserializeAsync<AppSettings>(stream, Options, cancellationToken).ConfigureAwait(false);
            if (settings is null || !SettingsPolicy.IsValid(settings))
                throw new SettingsFormatException("Settings contain unsupported values or schema version. The original file has been preserved.");
            return settings;
        }
        catch (JsonException error)
        {
            throw new SettingsFormatException("Settings JSON is invalid. The original file has been preserved.", error);
        }
        finally { gate.Release(); }
    }

    public async Task SaveAsync(AppSettings settings, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(settings);
        if (!SettingsPolicy.IsValid(settings)) throw new ArgumentException("Settings contain unsupported values.", nameof(settings));
        await gate.WaitAsync(cancellationToken).ConfigureAwait(false);
        string? temporaryPath = null;
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath)!);
            temporaryPath = fullPath + "." + Guid.NewGuid().ToString("N") + ".tmp";
            await using (var stream = new FileStream(temporaryPath, FileMode.CreateNew, FileAccess.Write,
                FileShare.None, 4096, FileOptions.Asynchronous | FileOptions.WriteThrough))
            {
                await JsonSerializer.SerializeAsync(stream, settings, Options, cancellationToken).ConfigureAwait(false);
                await stream.FlushAsync(cancellationToken).ConfigureAwait(false);
                stream.Flush(flushToDisk: true);
            }
            cancellationToken.ThrowIfCancellationRequested();
            // Both paths are on the same volume. Never delete the destination before replacement.
            if (File.Exists(fullPath)) File.Replace(temporaryPath, fullPath, null);
            else File.Move(temporaryPath, fullPath);
        }
        finally
        {
            try
            {
                if (temporaryPath is not null && File.Exists(temporaryPath)) File.Delete(temporaryPath);
            }
            finally { gate.Release(); }
        }
    }
}
