# F05 — Foundation review

Review date: 2026-11-25. Source publication decision: **deferred**. The owner subsequently authorized the first local Foundation commit. Local remote configuration, source push, uploaded source and releases remain deferred.

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

The initial local commit is authorized. GitHub source upload, CI execution, main rules, templates and installer/release work remain deferred until separate explicit authorization. F05 can close as local review plus an explicit publication decision; closing it does not mean publishing was performed. The public roadmap/guide must reflect the cards-only scope and the completed review.
