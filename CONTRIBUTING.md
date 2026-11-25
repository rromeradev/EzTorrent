# Contributing

EzTorrent is an MIT-licensed Windows x64 application. Foundation is intentionally a shell without a torrent engine. Discuss changes against the current milestone before adding dependencies or features.

## Development

1. Install the SDK selected by `global.json` on Windows x64.
2. Run the restore/build/test commands in README.
3. Keep Core independent of UI, engine, persistence libraries and external packages. Infrastructure references Core; Desktop composes both.
4. Add behavioral tests for persistence, scheduling and security boundaries. Use temporary directories and dispose handles. Never depend on public torrents or network traffic for unit tests.
5. Run the desktop for UI changes. Check light/dark/system themes, pt-BR/en-US, keyboard navigation and caption controls. Include actual evidence and remaining limits in a pull request.

Use English for code, comments, documentation, issues and commits. Add user-facing text to all `.resx` variants. Never embed secrets, private trackers, real magnet links or personal download paths in examples, tests or reports. Use fictional or public-domain test fixtures with explicit provenance when engine work starts.

## Workflow

Keep `main` stable. Use short-lived `feat/`, `fix/` or `docs/` branches and Conventional Commits when commits are authorized. Follow SemVer. The public GitHub repository contains a planning Project, milestones and issues. Application source remains local; the first Foundation commit is authorized, while source push, remote configuration and releases remain deferred; see docs/GITHUB.md. Do not commit, push, publish or configure external services without the repository owner's explicit request.

Pull requests should explain the triggering problem, resulting behavior, validation and practical limits. The Windows workflow restores, builds and tests without publishing. Review dependency licenses and advisories before upgrades; select exact central package versions.

## Reporting

Use the public planning issues today. The local bug/feature issue forms and pull request template will become available on GitHub after source publication is authorized. Include Windows edition/build, SDK/runtime version, reproduction and a sanitized error description. Do not attach settings or torrent metadata containing personal information. For a security concern, use GitHub private vulnerability reporting as described in SECURITY.md; do not publish sensitive details in issues.
