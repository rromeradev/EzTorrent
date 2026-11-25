# Requirements

## Product and delivery

EzTorrent is a free, open-source MIT BitTorrent desktop client. Target Windows 10/11 x64, with Microsoft-supported Windows/.NET combinations documented separately from technical compatibility. Use C# 14/.NET 10 LTS, WPF XAML native Fluent, the Windows accent and a shared app-drawn window frame retaining native behavior on every app-owned window/dialog; no OS title bars or app-opened native message/file dialogs. The interface supports pt-BR and en-US. Source, documentation and commits are English.

Production distribution is installer-only through future Velopack + GitHub Releases. Updates automatically check and install under a defined user-respecting policy, with package/manifest authentication and fail-closed tamper handling. Signing identity, trust-anchor provisioning/rotation, anti-rollback and recovery must be designed and tested before rollout; HTTPS or a checksum alone is not authentication. No update implementation exists in Foundation.

## Foundation acceptance (0.1)

- Exactly three production projects: Desktop, Core and Infrastructure; one xUnit test project.
- Core is independent. Infrastructure references Core. Desktop references both and owns Hosting/DI composition.
- Runnable shell and genuine empty transfer list; cards as the sole transfer view; preferences and quick first-run setup.
- Atomic JSON settings with schema validation. Missing files use defaults; invalid/future files are preserved and surfaced.
- System/light/dark themes and Windows accent. Detect Windows UI language initially; support `.resx` en-US/pt-BR and explicit override.
- Ask on the first close, remember a choice and allow changing it. Until tray support, minimization means taskbar.
- Telemetry is optional and off by default; absolutely no collection in this phase.
- Documentation, MIT license without invented ownership, Windows CI build/test, contribution guidance and issue forms.
- No functional engine. Do not present fake downloads, connections, speeds or working add-torrent actions.

## Required v1 capabilities (future unless above)

| Area | Agreed behavior |
| --- | --- |
| Input | Open `.torrent` files and magnet links; opt-in Windows file/protocol associations. Validate input before use. |
| Discovery | DHT, PEX, LSD and trackers, with capability and interoperability checks. |
| Transport | MSE/PE encryption support and configurable policy; no claim of anonymity. |
| Router mapping | UPnP/NAT-PMP, with explicit settings and graceful fallback. |
| Protocol versions | BitTorrent v1, v2 and hybrid only after validating the chosen MonoTorrent release and fixtures. |
| Confirmation | Select files and destination before payload download. Magnet metadata retrieval may precede confirmation; no selected content starts before approval. |
| Lifecycle | Queue, pause/resume/remove; removal offers a separate explicit option to delete downloaded files. Preserve resumable state. |
| Limits | Global download/upload limits; scheduling/queue policy designed before implementation. |
| Integrity | Hash checking, recheck controls and visible failure/recovery state. |
| Metrics | Progress, state, downloaded/uploaded bytes, rates, peers/seeds, ratio and ETA when meaningful. No fabricated values. |
| Storage | Check destination and available space; react to low space and I/O failures without silent data loss. |
| Seeding | Visible seeding state, ratio/time policies, limits and user control. |
| Resume | Engine-native Fast Resume alongside app JSON; version-aware, validated and crash-safe. JSON settings do not pretend to be resume data. |
| Windows | Optional tray and startup; user-selected post-download actions with explicit dangerous-action confirmation. Never execute torrent-supplied content or commands automatically. |
| Preferences | Quick setup wizard; configurable first-close behavior; responsive cards only; no table view or layout toggle; explicit language/theme selection. |
| Privacy | Anonymous optional telemetry off by default; meaningful consent and revocation before collection. Never collect torrent names/hashes, magnet links, trackers, local paths or user identifiers. |

## Security and correctness boundaries

Treat all torrent metadata, magnet parameters, tracker responses and file names as untrusted. Reject absolute/UNC/device paths, traversal (`..`), alternate data streams, reserved device names and normalized paths escaping the selected destination. Account for reparse points/symlinks, case-insensitive collisions, duplicate paths, long paths, concurrent filesystem changes and deletion boundaries. Never delete outside the explicitly selected torrent content root. Add adversarial fixtures before engine integration.

Private torrents must honor the private flag and tracker policy: disable public DHT, PEX and LSD for them, including transitions and resume. Never leak private tracker URLs or passkeys through logs/telemetry. MSE/PE is transport encryption, not a privacy guarantee. Decide resource limits for metadata, peers and disk activity before accepting untrusted sessions.

Settings remain local. No account, cloud sync, remote API or background network operation is introduced in Foundation. Future diagnostic logs must be local, bounded, redacted and documented; no logging framework is installed merely to reserve a future choice.

## Explicitly outside v1

Streaming, integrated search, RSS, plugins, web UI, remote control and cloud functionality.

## Support qualification

See README and [validation](VALIDATION.md). OS compatibility goals do not override Microsoft's support lifecycle. Windows 10 editions/builds outside current vendor support are best-effort candidates only and have not been tested here. Do not advertise broad Windows 10 production support based on compilation alone.
