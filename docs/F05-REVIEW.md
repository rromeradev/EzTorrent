# F05 — Foundation review

Review date: 2026-11-25. Source publication decision: **authorized and completed** after the initial local review. The owner authorized the first commit, then explicitly requested its GitHub push and branch-policy verification. Packages and releases remain future work.

## Result and superseding UI decisions

The Foundation was reviewed and the owner's requested corrections implemented locally. Cards are now the only transfer view. All app-owned windows/dialogs inherit AppWindow with app-drawn chrome; native message and folder dialogs were replaced. This supersedes earlier grid/cards and session-only column customization plans. Future U02 work improves responsive, accessible cards instead of reintroducing a table.

Caption hit targets use current geometry. Custom maximize hover/press/capture loss are synchronized; dragging off cancels the action. WindowChrome retains resizing/drag/system commands and Windows 11 Snap Layouts. Explicit monitor work-area limits keep the footer visible when maximized; the template root is separate from app frame layout.

Legacy Grid/Cards JSON is ignored and omitted on save without losing other settings. A custom asynchronous folder picker handles drives/parent/children and inline failures. A review-found stale selection after editing Location was reproduced by an STA WPF test and fixed. Visual testing also revealed dispatcher culture restoration in later dialogs; resource lookup now consistently uses configured app culture. This has a regression test and was rechecked visually in English.

## Evidence

- Restore succeeded; Release build: zero warnings/errors; xUnit: **20 passed, zero failed**.
- Legacy layout migration cases failed before implementation, then passed.
- Folder path edit test failed with the previous folder still selected, then passed after clearing stale selection.
- App-owned frames observed on setup/preferences/folder picker/close confirmation. No standard Windows title bar on these views.
- Main view observed in pt-BR/dark and en-US/light with no layout toggle/table.
- Physically moved and resized the window, maximized/restored, minimized, tested maximize press-drag-out-release cancellation and confirmed correct new caption positions.
- Snap Layouts popup observed; maximized footer was visible after explicit work-area handling.
- Close cancellation and confirmed Exit were exercised. Dialog text was verified in the configured English language after restart.
- Independent read-only review identified the stale folder selection and proposed a state-specific caption label. The folder defect was fixed. The existing localized caption label already says Maximize or restore and remains accurate for both states.

## Limits and deferred work

Only the available Windows 11 x64 build was visually tested. Windows 10, a supported-build matrix, mixed DPI/monitors, high contrast, complete screen-reader qualification and live system theme/accent changes remain pending. Native monitor sizing uses current monitor information but this is not evidence of a mixed-monitor test.

App-owned dialogs share the same chrome; not every startup/access failure was forced visually in this pass. Picker directory access is asynchronous; an OS/network call can take time to return after cancellation. No functional torrent data exists yet, so populated card performance/virtualization and engine actions remain U02/engine milestones, not verified Foundation functionality.

The initial commit [`96f0feb`](https://github.com/rromeradev/EzTorrent/commit/96f0feb63f9f0aaa752131629cc521f759d280a3) is public. Its [Windows CI run](https://github.com/rromeradev/EzTorrent/actions/runs/45674317801) passed restore, Release build and tests. The active [Protect main ruleset](https://github.com/rromeradev/EzTorrent/rules/28098342) requires pull requests and the GitHub Actions `build` check, with squash merges and no bypass actors. Templates are available in the published source. Installer/release work remains on the roadmap.
