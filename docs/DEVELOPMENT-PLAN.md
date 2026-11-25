# EzTorrent — Development guide

**F05 reviewed (2026-11-25):** Cards are the only transfer view. Every app-owned window/dialog uses the app-drawn frame; no standard Windows title bars, native message boxes or native folder/file dialogs. Caption movement/hover/cancel/work-area behavior was corrected. Local Release build has zero warnings/errors and 20 tests passed. Source publication remains deferred.

EzTorrent is a free, open-source MIT BitTorrent client planned for Windows 10/11 x64. The current Foundation is implemented locally and remains unpublished. This repository currently holds planning only: no application source, builds, installer or release has been uploaded.

## Keep the workflow simple

- **Todo:** a small, scoped task with acceptance criteria and known prerequisites.
- **In Progress:** the one task currently being implemented; prefer one active task at a time.
- **Done:** acceptance criteria are verified and the evidence/limitations are recorded.
- A milestone groups one usable outcome, not an arbitrary date. Complete prerequisites before starting dependent work. No deadlines are promised.
- Each issue is intended to produce one reviewable result. Split it further if implementation reveals that it is too large.
- Use the issue's acceptance checklist as the definition of done. Do not mark planned protocols, signing or platform support as implemented without evidence.

## Delivery sequence

1. **v0.1.0 Foundation:** application structure, atomic settings, Fluent shell, localization, tests and docs. Implemented and reviewed locally; source publication is explicitly deferred.
2. **v0.2.0 Torrent Engine:** validate MonoTorrent and integrate safe input, metadata, trackers/discovery, encryption, router mapping, resume and proven protocol versions.
3. **v0.3.0 Download Management:** confirm selected files, queue, pause/resume/remove, bandwidth limits, real metrics, disk safeguards and seeding.
4. **v0.4.0 UX:** complete screens, responsive cards, setup/preferences, accessibility and an explicit telemetry decision.
5. **v0.5.0 Windows Integration:** optional associations, tray, startup, notifications and safe completion actions.
6. **v0.6.0 Distribution:** publisher/trust decisions, Velopack installer, authenticated automatic updates, controlled build pipeline and recovery tests.
7. **v1.0.0-beta.1 Stabilization:** supported Windows matrix, interoperability/performance/security/accessibility tests, user guides and authorized beta feedback. Further prereleases use 1.0.0-beta.N.
8. **v1.0.0 Release:** final acceptance and a separately authorized signed stable release.

## Approved technical direction

- C# 14/.NET 10 LTS, native WPF XAML Fluent with Windows accent and a custom title bar retaining native behavior.
- Exactly three production projects: independent Core; Infrastructure depends on Core; Desktop depends on both and owns Hosting/DI. CommunityToolkit.Mvvm for presentation.
- JSON settings use atomic same-volume replacement. Future Fast Resume remains engine-native state, separate from settings JSON.
- pt-BR/en-US resources, initial user Windows UI language, light/dark/system themes, cards-only transfers, first-run setup and first-close choice.
- MIT; do not invent a legal copyright holder, publisher or signing identity.
- English source, documentation, issues and authorized commits. Conventional Commits and SemVer once code publication is authorized.

## Product safeguards

File/magnet metadata may be inspected before confirmation; payload download requires file/destination confirmation. Future features include queueing, lifecycle actions, global limits, hash checks, real metrics, disk checks and seeding. DHT/PEX/LSD must stay off for private torrents. Treat torrent paths as hostile; enforce traversal/deletion/reparse boundaries. v1/v2/hybrid support is conditional on actual engine validation. MSE/PE is not anonymity.

Telemetry is optional and off by default; none is collected in Foundation. A future decision must precede any collector. Never collect names, hashes, magnet links, trackers, local paths or identifiers. Velopack/GitHub Releases and automatic updates remain future work; authenticate package/manifests and defend against tampering/downgrades before rollout.

Targeting Windows 10/11 does not mean every edition/build is supported. Validate vendor-supported combinations before beta. Streaming, integrated search, RSS, plugins, web UI, remote control and cloud features are outside v1.

## Current publication boundary

The owner approved a public planning repository and project only. Keep application code local. The owner authorized the first local Foundation commit at F05 completion. Source push, local remote configuration, artifact uploads, production release automation and beta/stable publication still require a separate explicit instruction. Five Foundation tasks, including the F05 review and publication decision, are complete based on recorded local work and validation; this does not imply shared source or a published release.

Use this guide and the linked milestones/issues as the public planning source of truth. Keep the local ROADMAP and requirements aligned. Decision issues deliberately defer publisher identity, signing, telemetry collection and release policy until evidence and an owner decision are available.


## Planning links

- [Development board](https://github.com/users/rromeradev/projects/4/views/3)
- [Milestones and tasks](https://github.com/users/rromeradev/projects/4/views/4)
- [Milestone progress](https://github.com/rromeradev/EzTorrent/milestones)

## Task index

### [v0.1.0 — Foundation](https://github.com/rromeradev/EzTorrent/milestone/1)

- [F01: Bootstrap the three-project application architecture](https://github.com/rromeradev/EzTorrent/issues/2) — verified locally; source unpublished
- [F02: Persist private application settings atomically](https://github.com/rromeradev/EzTorrent/issues/3) — verified locally; source unpublished
- [F03: Build the localized Fluent shell and first-run preferences](https://github.com/rromeradev/EzTorrent/issues/4) — verified locally; source unpublished
- [F04: Establish Foundation tests, documentation and CI scaffolding](https://github.com/rromeradev/EzTorrent/issues/5) — verified locally; source unpublished
- [F05: Review Foundation and decide when to publish the source](https://github.com/rromeradev/EzTorrent/issues/6)

### [v0.2.0 — Torrent Engine](https://github.com/rromeradev/EzTorrent/milestone/2)

- [E01: Validate and select the MonoTorrent dependency](https://github.com/rromeradev/EzTorrent/issues/7)
- [E02: Reject unsafe torrent paths and bound metadata input](https://github.com/rromeradev/EzTorrent/issues/8)
- [E03: Load .torrent metadata without starting payload transfers](https://github.com/rromeradev/EzTorrent/issues/9)
- [E04: Resolve magnet metadata before payload confirmation](https://github.com/rromeradev/EzTorrent/issues/10)
- [E05: Connect trackers and redact private tracker data](https://github.com/rromeradev/EzTorrent/issues/11)
- [E06: Implement discovery with private-torrent isolation](https://github.com/rromeradev/EzTorrent/issues/12)
- [E07: Validate MSE/PE transport encryption policies](https://github.com/rromeradev/EzTorrent/issues/13)
- [E08: Add controlled UPnP and NAT-PMP port mapping](https://github.com/rromeradev/EzTorrent/issues/14)
- [E09: Persist validated Fast Resume and shut the engine down safely](https://github.com/rromeradev/EzTorrent/issues/15)
- [E10: Prove BitTorrent v1, v2 and hybrid compatibility](https://github.com/rromeradev/EzTorrent/issues/16)

### [v0.3.0 — Download Management](https://github.com/rromeradev/EzTorrent/milestone/3)

- [D01: Confirm destination and file selection before download](https://github.com/rromeradev/EzTorrent/issues/17)
- [D02: Implement a predictable download queue](https://github.com/rromeradev/EzTorrent/issues/18)
- [D03: Implement pause, resume and safe remove choices](https://github.com/rromeradev/EzTorrent/issues/19)
- [D04: Apply global upload and download limits](https://github.com/rromeradev/EzTorrent/issues/20)
- [D05: Expose real transfer metrics and hash rechecks](https://github.com/rromeradev/EzTorrent/issues/21)
- [D06: Handle low disk space and write failures safely](https://github.com/rromeradev/EzTorrent/issues/22)
- [D07: Implement visible and bounded seeding policies](https://github.com/rromeradev/EzTorrent/issues/23)

### [v0.4.0 — UX](https://github.com/rromeradev/EzTorrent/milestone/4)

- [U01: Complete Add Torrent and transfer-detail screens](https://github.com/rromeradev/EzTorrent/issues/24)
- [U02: Complete responsive cards and their interaction states](https://github.com/rromeradev/EzTorrent/issues/25)
- [U03: Polish setup, preferences and first-close behavior](https://github.com/rromeradev/EzTorrent/issues/26)
- [U04: Verify translations, themes and accessibility](https://github.com/rromeradev/EzTorrent/issues/27)
- [U05: Decide and document optional telemetry before collecting anything](https://github.com/rromeradev/EzTorrent/issues/28)

### [v0.5.0 — Windows Integration](https://github.com/rromeradev/EzTorrent/milestone/5)

- [W01: Add opt-in .torrent and magnet associations](https://github.com/rromeradev/EzTorrent/issues/29)
- [W02: Implement tray behavior and graceful background exit](https://github.com/rromeradev/EzTorrent/issues/30)
- [W03: Add optional startup with Windows](https://github.com/rromeradev/EzTorrent/issues/31)
- [W04: Add notifications and safe post-download actions](https://github.com/rromeradev/EzTorrent/issues/32)

### [v0.6.0 — Distribution](https://github.com/rromeradev/EzTorrent/milestone/6)

- [P01: Define publisher identity and update trust policy](https://github.com/rromeradev/EzTorrent/issues/33)
- [P02: Build the Windows x64 Velopack installer](https://github.com/rromeradev/EzTorrent/issues/34)
- [P03: Implement authenticated automatic updates](https://github.com/rromeradev/EzTorrent/issues/35)
- [P04: Prepare a controlled release build and artifact pipeline](https://github.com/rromeradev/EzTorrent/issues/36)
- [P05: Test install, uninstall, update and tamper recovery](https://github.com/rromeradev/EzTorrent/issues/37)

### [v1.0.0-beta.1 — Stabilization](https://github.com/rromeradev/EzTorrent/milestone/7)

- [B01: Qualify the supported Windows x64 matrix](https://github.com/rromeradev/EzTorrent/issues/38)
- [B02: Run protocol, performance and long-session regression tests](https://github.com/rromeradev/EzTorrent/issues/39)
- [B03: Review path safety, update trust and privacy end to end](https://github.com/rromeradev/EzTorrent/issues/40)
- [B04: Audit accessibility and complete user/recovery documentation](https://github.com/rromeradev/EzTorrent/issues/41)
- [B05: Run an explicitly authorized beta and triage feedback](https://github.com/rromeradev/EzTorrent/issues/42)

### [v1.0.0 — Release](https://github.com/rromeradev/EzTorrent/milestone/8)

- [R01: Review final v1.0.0 acceptance and release readiness](https://github.com/rromeradev/EzTorrent/issues/43)
- [R02: Publish v1.0.0 only after owner authorization](https://github.com/rromeradev/EzTorrent/issues/44)

## Next local development step

[E01 — Validate and select the MonoTorrent dependency](https://github.com/rromeradev/EzTorrent/issues/7) is the first planned local engine task after the verified Foundation. This setup does not start implementation. [F05 — Foundation review](https://github.com/rromeradev/EzTorrent/issues/6) is complete; the owner chose to defer source publication. This does not block local research or development. The first local Foundation commit is authorized. No push, source upload or release is authorized by this planning setup.

## Security reporting

[Report security concerns privately](https://github.com/rromeradev/EzTorrent/security/advisories/new). Private vulnerability reporting is enabled. Do not post private tracker credentials, torrent data, local paths or personal information in public issues.
