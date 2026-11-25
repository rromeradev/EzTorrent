# Foundation Implementation Plan

> For agentic workers: execute this plan inline using superpowers:executing-plans. User explicitly authorized implementation in the existing directory and prohibited commits and publication.

**Goal:** Deliver a runnable, honest Foundation shell for EzTorrent 0.1.
**Architecture:** Core defines settings and contracts without UI or external packages. Infrastructure provides atomic JSON persistence. Desktop is the WPF composition root with Hosting, DI and MVVM.
**Tech stack:** C# 14, .NET 10 LTS, native WPF Fluent, CommunityToolkit.Mvvm, xUnit v3.
**Spec:** User-approved Foundation request; consolidated in ../../REQUIREMENTS.md.

## Global constraints
- Work directly in C:/Users/reina/source/repos/EzTorrent. Preserve existing content.
- No commits, remote repository, push, installer, engine or telemetry collection.
- English source and documentation; pt-BR/en-US resources; Windows x64 only for Desktop.

## Review focus
- Corrupt or future-version settings must be preserved rather than silently overwritten.
- Concurrent/cancelled saves must leave complete JSON and no temporary files.
- Unsupported cultures and invalid persisted values must resolve predictably.
- Native caption behavior, DPI, keyboard access and system theme updates need visual checks.
- First-run and close prompts must retain control and surface persistence errors.

## Tasks
1. [x] Add solution/build configuration and tests before persistence implementation. Verify the tests fail for missing contracts. Implement AppSettings, SettingsPolicy, ISettingsStore and JsonSettingsStore. Verify defaults, cultures, round-trip, replacement, corrupt/future data, concurrency and cancellation.
2. [x] Add WPF manifest, resources, localization, theme service, Hosting/DI startup, MainViewModel, native-chrome shell, empty grid/cards and preferences/first-run dialog. Build with warnings as errors. Verify no engine-facing actions pretend to work.
3. [x] Document requirements, architecture, UX, roadmap, contribution and MIT licensing without attributing an invented owner. Add Windows CI and issue templates.
4. [x] Restore/build/test, audit dependencies, launch the application, inspect the UI where possible, review the entire change, and record actual validation and remaining limits.

## Decisions and progress
- Initial inspection: target folder exists and is empty; no Git repository or inherited AGENTS.md found.
- Work in place, without an isolated worktree, as explicitly requested.
- MIT notice intentionally leaves copyright attribution unassigned; contributors retain applicable rights.
- Use WindowChrome with accessible SystemCommands caption buttons. Native DWM buttons were absent in the first visual run; Windows 11 HTMAXBUTTON plus captured-release handling now provides Snap Layouts and working maximize/restore.
- Language changes take effect on restart; theme/layout changes apply immediately.
- First-close offers exit, minimize to taskbar, or cancel. Tray support remains future work.

- Task 1 complete: persistence tests failed for missing contracts, then 14 tests passed after implementation. xUnit v3 cancellation diagnostics were resolved using TestContext.Current.CancellationToken.
- Task 2 complete: WPF built with zero warnings/errors; visual runs confirmed wizard, saved settings, empty states, layout switch and custom caption controls.
- Task 3 complete: product/architecture/UX docs, MIT text, roadmap, contribution/agent guidance, issue forms and Windows build/test workflow exist.
- Final independent review found Windows installation culture selection and invalid persisted absolute paths; both fixed. Path regression tests failed before the fix, then the full 16-test suite passed.
- Final verification: restore succeeded, Release build 0 warnings/0 errors, xUnit 16 passed/0 failed, NuGet direct/transitive advisory scan reported no known vulnerable packages.
- Native maximize click failed in the first hit-test-only implementation; explicit nonclient press/captured-release routing fixed it. Physical click maximize/restore, Snap popup, minimize and keyboard column visibility were observed.
- No commits, remote configuration or publication. Work project registration is unavailable through the exposed tools; implementation remains in the explicitly selected directory.
- Source action tags v7/v6 were confirmed with git ls-remote and pinned to the returned immutable commits. CI itself was not run on GitHub.
- No deferred review findings; remaining platform/accessibility/engine work is documented in VALIDATION.md and ROADMAP.md.
