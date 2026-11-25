# F05 window and cards review

**Goal:** Finish the local Foundation review with app-drawn chrome on every app-owned window and a cards-only transfer view. The owner keeps source publication deferred; no commits, remotes or push.

## Requested corrections
- No OS-drawn title bars/borders on Main, Preferences, setup, folder picker, confirmations or errors.
- Caption hover/press/click targets follow window movement, resizing, maximization and DPI. Preserve drag, resize, Alt+F4, system menu and Windows 11 Snap behavior.
- Cards are the only transfer presentation. Remove table controls and layout commands/settings; accept and discard legacy layout JSON without losing other preferences.

## Findings
Preferences derives from ordinary Window; MessageBox and OpenFolderDialog introduce OS-owned chrome. Main combines its custom controls with default WindowStyle. The maximize hook routes only selected nonclient messages and never updates app button hover/press state. Separate caption implementations also make behavior inconsistent.

## Implementation and verification
- [x] Add a failing persistence regression covering legacy Grid/Cards preferences; remove the obsolete layout model and UI.
- [x] Introduce one reusable AppWindow template with WindowStyle=None, app caption controls, WindowChrome resize/drag and dynamic hit testing. Handle hover/press/capture loss consistently without caching screen rectangles.
- [x] Use AppWindow for preferences/setup, messages and a custom folder picker. Never restyle an OS-owned dialog or silently launch one.
- [x] Restore/build/test. Visually check main/setup/preferences/folder/message windows, caption movement/hover/press/cancel, resize/maximize/restore/minimize, light/dark and language resources using isolated data.
- [x] Review the changes independently; document real limits and the cards-only superseding decision. Update F05 and affected roadmap issues; close the review with publication explicitly deferred.

No engine, updater, telemetry, tray or unrelated application features are added. OS-owned security prompts and external applications are outside app chrome ownership; Foundation launches none.

## Review record

Legacy layout regression RED→GREEN; STA folder selection regression RED→GREEN; configured language regression added; final suite 20/20. Independent review completed. Maximize clipping discovered visually and corrected with monitor work-area sizing. Locale mismatch in later dialogs corrected and visually verified. See docs/F05-REVIEW.md for evidence and deferred matrix. Publication remains deferred.

The final owner instruction authorizes the first local Foundation commit. It does not authorize a source push.
