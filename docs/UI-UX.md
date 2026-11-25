# UI / UX

## Current owner decisions

Cards are the only transfer presentation. There is no table, grid/column configuration, toggle or saved layout preference. This F05 decision supersedes the earlier plan for two layouts.

Every app-owned window uses the same AppWindow template: main, setup/preferences, folder picker, confirmations, startup errors and save errors. Use an app-drawn caption and border with native Fluent content/system accent. No MessageBox or native file/folder dialog may introduce a Windows title bar. External applications and OS security UI are not app-owned; Foundation launches neither.

## Foundation flow

Read settings and the user's Windows UI language before constructing views. First startup shows a short setup form; Save marks it complete and Cancel leaves it pending. No engine, network request, association, startup registration or collection occurs.

The main sidebar contains All torrents and Preferences. Add torrent is disabled with an availability tooltip. A genuine empty card state explains the 0.2 engine milestone. There are no sample downloads, fabricated speeds or fake connections. Future cards display real names/progress/state/rates and accessible actions; responsive behavior and performance are part of U02.

Preferences save atomically and keep validation/save failures visible. Light/dark/system apply after save; an explicit language change applies after restart. All subsequently opened dialogs use the configured language consistently across dispatcher callbacks. The telemetry control stays disabled because no collection exists.

Folder selection uses an app-owned asynchronous browser. Users can browse drives/parent/children, type an absolute location, select a folder or cancel. Directory reads do not block the dispatcher, are cancellable between entries, and show access failures inline. No directories are created. Editing Location clears any previous folder selection to avoid silently choosing a stale destination. Slow OS/network directory enumeration may take time to return even after cancellation; the closed dialog ignores late results.

First close offers clearly labeled Exit, Minimize to taskbar and Cancel buttons in an app-owned dialog; Cancel is the default. Exit/Minimize are remembered only after a successful save. Cancel/title-close leave the main window open without changing settings. Preferences can restore Ask. Minimize does not hide the app in a tray or start background transfers. Future tray/seeding work must keep these choices explicit.

## Frame and controls

WindowStyle=None removes the OS caption. WindowChrome retains drag, double-click, resize, keyboard/system commands and OS snapping. Custom buttons have localized accessible labels and follow the current layout. Windows 11 HTMAXBUTTON enables Snap Layouts; nonclient hover/press/capture-loss state is reflected in the app button. Pressing maximize and releasing outside cancels the action. Work-area sizing prevents content being hidden behind the taskbar. Never cache caption screen rectangles or overlay a second set of OS caption buttons.

## Remaining qualification

Current smoke tests cover the available Windows 11 build, pt-BR/en-US, light/dark, setup/preferences/folder/close frames, movement, resize, maximize/restore, minimize, Snap and drag-off cancellation. Windows 10, mixed DPI/monitors, live system theme/accent changes, high contrast and full screen-reader coverage remain future qualification. See F05-REVIEW.md and VALIDATION.md; do not imply this larger matrix passed.
