namespace EzTorrent.Core.Settings;

public interface ISettingsStore
{
    Task<AppSettings> LoadAsync(CancellationToken cancellationToken = default);
    Task SaveAsync(AppSettings settings, CancellationToken cancellationToken = default);
}

public sealed class SettingsFormatException(string message, Exception? innerException = null)
    : Exception(message, innerException);
