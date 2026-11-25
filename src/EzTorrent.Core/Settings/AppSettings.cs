namespace EzTorrent.Core.Settings;

public enum AppTheme { System, Light, Dark }
public enum CloseBehavior { Ask, Exit, Minimize }

public sealed record AppSettings
{
    public const int CurrentSchemaVersion = 1;
    public int SchemaVersion { get; init; } = CurrentSchemaVersion;
    public AppTheme Theme { get; init; } = AppTheme.System;
    public CloseBehavior CloseBehavior { get; init; } = CloseBehavior.Ask;
    public string? Language { get; init; }
    public string DownloadDirectory { get; init; } = "";
    public bool TelemetryEnabled { get; init; }
    public bool HasCompletedSetup { get; init; }
}
