# EzTorrent agent guidance

## Scope and authorization
- Implement only the requested milestone. Foundation has no functional torrent engine, updater, tray integration, file associations or telemetry collection.
- Inspect the working tree and existing instructions before changes. Preserve existing work.
- Work directly in the user-selected checkout unless they request isolation.
- Do not commit, create a remote repository, push, publish, send messages or create external issues without an explicit user request.
- Use English for code, documentation and authorized commit messages. Keep pt-BR/en-US UI text in resources.

## Architecture
- `EzTorrent.Core`: plain .NET 10, no external/project dependencies; domain/settings contracts only.
- `EzTorrent.Infrastructure`: depends on Core only. Future MonoTorrent adapters belong here, never in Core or views.
- `EzTorrent.Desktop`: .NET 10 WPF x64; composition root using Hosting/DI; CommunityToolkit.Mvvm for presentation state/commands.
- `EzTorrent.Tests`: xUnit v3 on Windows; isolate filesystem tests, use STA for WPF regressions, and never connect live peers/trackers.
- Exactly three production projects. Do not introduce a service layer project, repository abstraction framework, CQRS or plugin system without a demonstrated requirement.

## Implementation
- Centralize exact NuGet versions in `Directory.Packages.props`. Respect C# 14, nullable references, warnings as errors and `.editorconfig`.
- Native WPF Fluent and system accent; no third-party UI framework. WindowChrome must retain resize, drag, keyboard and OS maximize behavior. Every app-owned window/dialog must derive from AppWindow and use its app-drawn frame. Do not introduce MessageBox, OpenFileDialog, OpenFolderDialog or another OS-framed app dialog. Cards are the only transfer view; never restore a table/grid toggle or layout preference.
- Missing settings use privacy-preserving defaults. Corrupt/future settings remain intact and produce actionable startup failure. Never delete the destination before an atomic same-volume replacement.
- Keep telemetry off by default. No collection in Foundation. Future opt-in must exclude torrent hashes, names, magnet links, trackers, local paths and identifiers.
- Treat torrent metadata and paths as untrusted. Future private-torrent mode must disable DHT/PEX/LSD and honor tracker restrictions.
- Do not claim protocol support, package authenticity or supported Windows versions without verification.

## Verification
Run `dotnet restore EzTorrent.slnx`, `dotnet build EzTorrent.slnx --no-restore -c Release` and `dotnet test EzTorrent.slnx --no-build --no-restore -c Release` from this checkout. Inspect the actual results. For UI edits, run the application using an isolated `EZTORRENT_DATA_DIRECTORY` and inspect caption controls, keyboard access, themes and translations where feasible. Record limitations in `docs/VALIDATION.md`; never substitute a successful build for visual validation.
