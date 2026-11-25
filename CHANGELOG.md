# Changelog

This project follows Semantic Versioning. No release has been published.

## [Unreleased] — 0.1.0 Foundation

### Added
- Public Foundation source and planning Project with eight milestones, 43 scoped tasks, native prerequisite links and a pinned guide.
- Active main rules requiring pull requests, successful Windows CI and squash merges, with no force pushes, branch deletion or bypass actors.
- Local development/GitHub guides, security reporting policy and a prepared pull request template.
- .NET 10/C# 14 solution with independent Core, Infrastructure, WPF Desktop and xUnit tests.
- Native Fluent shell with Windows accent, custom caption controls, cards-only transfers and a shared app-owned frame for all windows/dialogs.
- JSON preferences using temporary-file flush and atomic replacement, invalid-data preservation, schema validation and concurrent-write serialization.
- Light/dark/system appearance, pt-BR/en-US resources, Windows language detection and preferences.
- Quick first-run configuration and remembered first-close exit/minimize choice.
- MIT license text, architecture/requirements/UX documentation, contribution guidance, roadmap and Windows build/test workflow.

### Fixed in F05 review
- Replaced OS-framed preferences, messages and folder picker with one reusable app-owned window frame.
- Corrected caption hover/press/cancel routing after move/resize and respected monitor work area on maximization.
- Prevented stale folder selection after editing its path and kept dialog resources in the selected app language.
- Added legacy layout, folder-picker STA and cross-context localization regressions; current suite has 20 tests.

### Known limitations
- No torrent engine, downloads, Fast Resume, installer, updater, tray/startup integration, Windows associations or telemetry collection.
- Language changes need restart. Cards are the only transfer view; legacy layout preferences are discarded without losing other settings.
- No published packages or releases. Windows 10 and multi-DPI/multi-monitor verification remain pending.
