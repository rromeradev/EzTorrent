using System.Globalization;

namespace EzTorrent.Core.Settings;

public static class SettingsPolicy
{
    public static string ResolveLanguage(string? preference, CultureInfo windowsCulture)
        => preference is "pt-BR" or "en-US" ? preference
            : windowsCulture.TwoLetterISOLanguageName == "pt" ? "pt-BR" : "en-US";

    public static bool IsValidDownloadDirectory(string? path)
        => path is not null && (path.Length == 0 ||
            (Path.IsPathFullyQualified(path) && path.IndexOfAny(Path.GetInvalidPathChars()) < 0
             && path.IndexOfAny(['<' , '>' , '"' , '|' , '?' , '*']) < 0
             && path.IndexOf(':', 2) < 0));

    public static bool IsValid(AppSettings settings)
        => settings.SchemaVersion == AppSettings.CurrentSchemaVersion
           && Enum.IsDefined(settings.Theme)
           && Enum.IsDefined(settings.CloseBehavior)
           && settings.Language is null or "pt-BR" or "en-US"
           && IsValidDownloadDirectory(settings.DownloadDirectory);
}
