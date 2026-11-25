# Roadmap

Track the [development board](https://github.com/users/rromeradev/projects/4/views/3), [milestones](https://github.com/rromeradev/EzTorrent/milestones) and [scoped task index](https://github.com/rromeradev/EzTorrent/issues/1). Public planning does not authorize source publication.

F05 completed the Foundation review. The owner subsequently authorized source publication; the initial commit is public and Windows CI passed. No package or release has been published. The latest UI decision supersedes the original grid/cards plan: cards only, and app-drawn chrome for every app-owned window/dialog.

Versions describe milestones, not promised dates. All milestones after Foundation are unimplemented. Acceptance requires tests and explicit verification; mentioning a capability does not claim engine support.

| Milestone | Outcome and acceptance |
| --- | --- |
| **0.1 Foundation** | Three-project architecture, WPF Fluent shell, real empty cards, local settings, themes, localization, first-run/close flows, tests and documentation. No transfers. |
| **0.2 Torrent Engine** | Validate MonoTorrent package/API/license compatibility; integrate `.torrent` and magnet metadata, trackers, discovery, encryption, hashing and Fast Resume behind Core contracts. Validate v1/v2/hybrid with fixtures before claiming support. |
| **0.3 Download Management** | Confirm files before payload download, queue, pause/resume/remove, explicit delete-files choice, global limits, metrics, disk-space safeguards, seeding policies and recovery. |
| **0.4 UX** | Complete add-torrent and details flows, progress/error accessibility, responsive cards and their interaction states, polish wizard/preferences and translation/theme coverage. |
| **0.5 Windows Integration** | Opt-in `.torrent`/magnet associations, tray, startup with Windows, notifications and explicit safe post-download actions. |
| **0.6 Distribution** | Installer-only distribution with Velopack and GitHub Releases, automatic checks/install, authenticated packages/manifests, trusted signing, tamper rejection, downgrade defense and update recovery. |
| **1.0 beta — Stabilization** | Protocol interoperability, supported Windows matrix, accessibility, performance, privacy/security review, regression/soak testing and beta feedback. Use `1.0.0-beta.N` prerelease versions. |
| **1.0 Release** | Stable verified v1 scope, documented supported platforms, authenticated installer/update chain, release notes and recovery procedures. |

## Outside v1

Streaming, integrated torrent search, RSS, plugins, web UI, remote control and cloud features. Reconsider only through a new scope decision.
