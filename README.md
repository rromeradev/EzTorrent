# EzTorrent

A free, open-source BitTorrent client for Windows, licensed under MIT.

[Public planning](https://github.com/users/rromeradev/projects/4/views/3) · [Project guide](https://github.com/rromeradev/EzTorrent/issues/1) · [GitHub setup](docs/GITHUB.md)

GitHub currently hosts planning only. Application source remains local; the owner authorized the first local Foundation commit; source push, packages and releases remain deferred. See [development plan](docs/DEVELOPMENT-PLAN.md) for the staged task index.


**Status: 0.1 Foundation, unreleased.** This version opens a native WPF Fluent shell, saves local preferences and uses cards as its only transfer view. It cannot add torrents, connect to peers, download or seed. All transfer collections are genuinely empty. No telemetry is collected and no updater or engine is installed.

## Build and run

Use Windows x64 with the .NET SDK selected by `global.json` (10.0.302, latest patch in that feature band). C# 14 and .NET 10 LTS are required. The SDK includes WPF build tools; Visual Studio is optional and must support this SDK. NuGet access is required for the first restore.

```powershell
dotnet restore EzTorrent.slnx
dotnet build EzTorrent.slnx --no-restore -c Release
dotnet test EzTorrent.slnx --no-build --no-restore -c Release
dotnet run --project src/EzTorrent.Desktop -c Release --no-build
```

The Desktop executable is x64 and framework-dependent; it requires the .NET 10 **Desktop Runtime**, not just the base runtime. Production distribution will use an installer in milestone 0.6; this source build is a development preview.

## Platform support

The product targets Windows 10/11 x64. Technical compatibility is distinct from vendor support. Microsoft's current [.NET Windows support matrix](https://learn.microsoft.com/dotnet/core/install/windows#supported-versions) limits supported Windows 10 editions/releases to eligible LTSC/Enterprise variants within their lifecycle. Ordinary Windows 10 Home/Pro 22H2 is not claimed as a supported production platform. Prefer an actively supported Windows 11 x64 release. Neither x86 nor ARM64 is a product target, including emulation. Windows 7/8, Linux and macOS are outside scope. No Windows 10 VM or multi-release Windows matrix has been tested in Foundation. Mica, rounded corners and Snap Layouts depend on OS support; Windows 10 uses solid surfaces and ordinary snapping. See [validation](docs/VALIDATION.md) for the actual machine tested.

## What works

- Native WPF Fluent controls, system accent resources, light/dark/system themes.
- One app-drawn frame for every app-owned window: main, setup/preferences, confirmations/errors and folder picker. Native drag/resize/system-menu/Snap behavior is retained without an OS title bar.
- Cards-only transfer presentation with a genuine empty state. There is no table, layout toggle or stored layout preference.
- Quick first-run configuration, preferences and a remembered first-close choice.
- `.resx` English and Brazilian Portuguese; Windows UI language detection. Language changes apply after restart.
- Serialized atomic JSON settings writes under `%LOCALAPPDATA%/EzTorrent/settings.json`. Invalid settings stop startup and remain untouched; review/rename the file to recover. No silent reset or automatic migration.
- Telemetry disabled by default; Foundation contains no collection or outbound networking code.

For isolated manual testing, set `EZTORRENT_DATA_DIRECTORY` to an absolute folder before launch. The application stores `settings.json` there instead of in the profile. Do not point this at valuable documents. A per-session mutex prevents multiple EzTorrent instances. Minimization currently uses the taskbar; tray support is planned.

## Repository

| Path | Responsibility |
| --- | --- |
| `src/EzTorrent.Core` | Framework-independent settings and contracts; no project/package dependencies |
| `src/EzTorrent.Infrastructure` | JSON persistence; depends only on Core |
| `src/EzTorrent.Desktop` | WPF, MVVM and Hosting/DI composition root; references both |
| `tests/EzTorrent.Tests` | xUnit tests for Core, Infrastructure and Windows UI regressions |

Read [requirements](docs/REQUIREMENTS.md), [architecture](docs/ARCHITECTURE.md), [UI/UX](docs/UI-UX.md), [roadmap](ROADMAP.md), [changelog](CHANGELOG.md) and [contribution guide](CONTRIBUTING.md).

## License and attribution

See [LICENSE](LICENSE). No legal copyright holder has been asserted on behalf of the user. Contributors retain their applicable rights; attribution must use verified information. Third-party packages retain their own licenses. Source, technical documentation and future commits use English; UI translations use the target language.
