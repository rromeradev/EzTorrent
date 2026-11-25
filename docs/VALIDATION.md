# Foundation validation and handoff

**F05 review (2026-11-25):** The owner authorized the first local Foundation commit at the end of F05; source publication is still deferred. Current evidence is in [F05-REVIEW.md](F05-REVIEW.md): cards only; shared app-owned frame/dialogs; caption/work-area fixes; 20 tests passing. Earlier grid/table and 16-test statements below are the historical first Foundation handoff.

**Planning update (2026-11-25):** GitHub planning was subsequently configured without commits or source publication. See [GitHub setup](GITHUB.md) for the verified current state. The Foundation handoff below records the original implementation state.

Date: 2026-11-25. Repository: `C:/Users/reina/source/repos/EzTorrent`.

## Result

Implemented the approved 0.1 Foundation directly in the requested folder. Initial elevated inspection found an empty directory, no Git repository and no inherited AGENTS.md. No existing user files were overwritten. Initialized local Git on `main`; no commits, staging, remotes, publication or GitHub repository creation.

The solution contains three production projects and one xUnit project. Core has no project/package dependencies. Infrastructure references Core only. Desktop references both and owns WPF, MVVM, Hosting/DI and application lifecycle. The UI has a real empty source, grid/cards, disabled Add Torrent, preferences, first-run setup and remembered first-close behavior. No engine, peer/tracker traffic, telemetry collection, installer, updater, file association, tray or startup registration exists.

## Verified environment and commands

- Windows 11 Pro x64, build 10.0.26300; this is the only OS build visually tested.
- .NET SDK 10.0.302; .NET and WPF reference/runtime 10.0.10 installed locally.
- `dotnet restore EzTorrent.slnx`: succeeded.
- `dotnet build EzTorrent.slnx --no-restore -c Release`: succeeded, 0 warnings, 0 errors.
- `dotnet test EzTorrent.slnx --no-build --no-restore -c Release`: **16 passed, 0 failed, 0 skipped**.
- `dotnet list EzTorrent.slnx package --vulnerable --include-transitive`: no known vulnerable direct/transitive packages reported by configured NuGet sources at validation time. This is an advisory lookup, not a security audit.
- Direct package versions successfully restored: CommunityToolkit.Mvvm 8.4.0, Microsoft.Extensions.Hosting 10.0.8, Microsoft.NET.Test.Sdk 18.0.1, xunit.v3 3.2.2, xunit.runner.visualstudio 3.1.5.
- Official GitHub action tags checked with `git ls-remote`: checkout v7 -> `3d3c42e5aac5ba805825da76410c181273ba90b1`; setup-dotnet v6 -> `0f4a354dd5547a28c70b7853dbd95d5b67886e73`. Workflow pins these commits. It is prepared locally and has not executed on GitHub.
- All three `.resx` variants have the same resource keys and no duplicate keys.

## Behavioral test coverage

Missing settings/default privacy and first-run state; theme/layout/close defaults; Portuguese/English/unsupported Windows language fallback and explicit override; round-trip and atomic replacement; malformed/null/future-schema/invalid-enum/unsupported-language preservation; simultaneous writes; cancelled writes preserving the previous document; invalid absolute path characters rejected without altering the original JSON. Regression cases were observed failing before the validation fix.

## Visual validation

Launched the Release executable using `EZTORRENT_DATA_DIRECTORY=C:/Users/reina/source/repos/EzTorrent/artifacts/visual-smoke` to isolate preferences from the user's normal profile. Observed Portuguese first-run setup, completion and persisted JSON; genuinely empty grid/cards; persisted layout across restart; light and dark Fluent rendering; theme selection; disabled telemetry and Add Torrent; accessible caption controls; native Windows 11 Snap Layouts popup; physical-click maximize and restore; taskbar minimization; Columns menu with keyboard-driven column hiding; first-close question and saved Exit choice. A test process was stopped after minimization because the UI automation helper could not reactivate a minimized window; no unsaved user data or downloads existed.

The first visual pass revealed absent DWM caption buttons. Replaced them with accessible custom SystemCommands buttons. A hit-test-only maximize implementation exposed Snap Layouts but did not act on clicks; captured nonclient press/client release routing fixed this and maximize/restore was then observed.

An independent read-only code review found two issues: original installation culture was used instead of the user's display culture, and invalid absolute paths from JSON bypassed validation. The app now captures the original CurrentUICulture before applying overrides; Core shares directory validation with preferences. The path regression suite passes. No review findings remain open.

## Decisions and limits

- Code, documentation and future commits are English; UI resources are pt-BR/en-US. Language changes apply after restart. Theme and grid/cards changes apply after successful save.
- First-close choice is Ask/Exit/Minimize to taskbar. Tray support remains a future milestone; no hidden background application is implied.
- Column visibility, order, width and sort are session-only in Foundation. Persistence is planned for 0.4.
- MIT permission text is present without inventing a legal owner/publisher. Actual copyright attribution remains unassigned pending verified information.
- Fast Resume is future engine-native state alongside app JSON. MonoTorrent, Velopack and signing trust are planned, not installed or implemented. Signing identity, authenticated package/manifest chain, anti-rollback and key rotation must be validated before automatic updates.
- Programmatic WPF ThemeMode still carries an experimental diagnostic in the installed SDK; suppression is scoped only to that access.
- A single-session mutex and single settings store serialize normal app access. Cross-session/process filesystem coordination and migrations are not implemented. Atomic replacement is verified on the local filesystem; power loss can leave orphan temporary files and hardware durability is not guaranteed.
- No Windows 10 VM, multiple supported Windows builds, full en-US visual pass, automatic Windows UI-language switching scenario, live system theme/accent change, mixed DPI/multiple monitors, high contrast or screen-reader matrix was tested. The available system and static resource checks do not establish that wider support.
- Windows 10 product targeting does not imply vendor support for all editions. Microsoft's current .NET support table limits Windows 10 support to eligible LTSC/Enterprise releases within lifecycle; ordinary Home/Pro 22H2 is not claimed as a supported production platform.

## Work project registration

The exposed app tools can list existing projects and move chats into existing projects, but expose no create/import-local-project operation. EzTorrent was absent from the returned project list. The available desktop surface is ChatGPT and cannot be registered through the allowed app operations here. Therefore **no dedicated Work project/workspace was created**. This is a project-registration limitation only: local filesystem access and implementation in the requested repository succeeded. The current chat's original workspace directory was not changed by a project tool; all repository operations explicitly targeted the requested directory.

## Files

- Root: README.md, LICENSE, CONTRIBUTING.md, ROADMAP.md, CHANGELOG.md, AGENTS.md, .editorconfig, .gitignore, Directory.Build.props, Directory.Packages.props, global.json, EzTorrent.slnx.
- docs: REQUIREMENTS.md, ARCHITECTURE.md, UI-UX.md, VALIDATION.md and the completed Foundation plan in docs/superpowers/plans.
- src/EzTorrent.Core: settings record/enums, validation policy and persistence contract.
- src/EzTorrent.Infrastructure: atomic JsonSettingsStore.
- src/EzTorrent.Desktop: WPF app/manifest, MainViewModel, main/preferences views, localization/resources, settings/theme/dialog services and native caption behavior.
- tests/EzTorrent.Tests: xUnit v3 project and persistence/policy regression tests.
- .github: Windows build/test workflow and bug/feature issue forms.

Build outputs and isolated visual-smoke settings are Git-ignored. No runtime preferences or downloaded payloads are included in source.

## References checked

- [WPF Fluent themes, ThemeMode and accent](https://learn.microsoft.com/dotnet/desktop/wpf/whats-new/net90)
- [WPF .NET 10](https://learn.microsoft.com/dotnet/desktop/wpf/whats-new/net100)
- [.NET Windows support matrix](https://learn.microsoft.com/dotnet/core/install/windows#supported-versions)
- [WindowChrome](https://learn.microsoft.com/dotnet/api/system.windows.shell.windowchrome?view=windowsdesktop-10.0)
- [CurrentUICulture](https://learn.microsoft.com/dotnet/api/system.globalization.cultureinfo.currentuiculture?view=net-10.0)
- [xUnit v3](https://xunit.net/docs/getting-started/v3/getting-started)
- [Official checkout action](https://github.com/actions/checkout), [setup-dotnet action](https://github.com/actions/setup-dotnet)
- [Planned MonoTorrent engine](https://github.com/alanmcgovern/monotorrent)
- [Future Velopack distribution](https://docs.velopack.io/), [code signing](https://docs.velopack.io/packaging/signing)
