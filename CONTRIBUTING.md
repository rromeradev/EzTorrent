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

Keep `main` stable. Use short-lived `feat/`, `fix/` or `docs/` branches and Conventional Commits. Follow SemVer. Open a pull request for every change after the initial repository bootstrap. The active `Protect main` ruleset requires the GitHub Actions `build` check against the latest `main`, resolved review conversations and a linear history. Only squash merges are allowed; merged branches are deleted automatically. Force pushes and branch deletion are blocked, with no configured bypass actors. A separate approving reviewer is optional so the solo maintainer can merge a verified pull request. See [GitHub setup](docs/GITHUB.md).

The owner authorized Foundation source publication. Agents still need an explicit task instruction before committing, pushing, publishing releases or configuring external services. Source publication does not authorize a package, installer or beta/stable release.

Pull requests should explain the triggering problem, resulting behavior, validation and practical limits. The Windows workflow restores, builds and tests without publishing. Review dependency licenses and advisories before upgrades; select exact central package versions.

## Reporting

Use the public bug/feature issue forms and pull request template. Include Windows edition/build, SDK/runtime version, reproduction and a sanitized error description. Do not attach settings or torrent metadata containing personal information. For a security concern, use GitHub private vulnerability reporting as described in SECURITY.md; do not publish sensitive details in issues.
