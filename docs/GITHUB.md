# GitHub setup and F05 completion

Updated 2026-11-25.

- [Public source and planning repository](https://github.com/rromeradev/EzTorrent)
- [Development board](https://github.com/users/rromeradev/projects/4/views/3)
- [Tasks grouped by milestone](https://github.com/users/rromeradev/projects/4/views/4)
- [Pinned guide](https://github.com/rromeradev/EzTorrent/issues/1)
- [Completed F05 review](https://github.com/rromeradev/EzTorrent/issues/6)

The owner authorized the first Foundation commit and subsequently requested its GitHub push and branch-policy verification. Commit [`96f0feb`](https://github.com/rromeradev/EzTorrent/commit/96f0feb63f9f0aaa752131629cc521f759d280a3) is public on `main`, and local `origin` points to this repository. The initial [Windows CI run](https://github.com/rromeradev/EzTorrent/actions/runs/45674317801) passed restore, Release build and tests. Issue forms and the pull request template are available. No packages, installer, tag or release have been published.

The Project contains 43 development tasks plus a separate pinned guide issue. Five Foundation tasks are closed/Done; 38 future tasks remain Todo. Foundation milestone v0.1.0 is closed as a local implementation/review, with no published release. The other seven milestones remain open. No due dates were invented. The board columns use Status (Todo, In Progress, Done); the table groups by Milestone. Native blockers record 77 future prerequisite relationships. Closing F05 does not block local research; E01 / issue #7 is the next planned task, not started by this setup.

F05 supersedes the grid/cards plan: cards only, and a shared app-drawn frame on every app-owned window/dialog. F03, U02, the pinned guide and Project README were updated accordingly. Detailed evidence and platform limits are in F05-REVIEW.md. Restore and Release build succeeded with zero warnings/errors; 20 tests passed.

Issues/Projects are enabled, wiki/discussions disabled, fifteen custom labels configured, guide #1 pinned, private vulnerability reporting enabled, and the repository homepage points to the board. Future merges use squash with branch cleanup. No collaborators, secrets, deploy keys, paid services or publishing automation were added.

## Enforced branch policy

The [Protect main ruleset](https://github.com/rromeradev/EzTorrent/rules/28098342) is active for `refs/heads/main`, with no configured bypass actors, including the owner. The initial root commit bootstrapped the empty repository before protection was enabled. Subsequent changes use short-lived `feat/`, `fix/` or `docs/` branches and pull requests.

- Pull requests are required. A separate approving reviewer is optional for solo development; review conversations must be resolved.
- The `build` check must succeed against the latest `main`. It is restricted to GitHub Actions (integration ID 15368), verified from the successful initial run.
- History must stay linear. Pull requests use squash merges only; merged branches are deleted automatically.
- Force pushes and deletion of `main` are blocked.

The effective branch-rules API confirmed all five rules. The GitHub plugin handles supported repository/issue/PR operations; the authenticated official GitHub CLI complements Projects and repository administration, rulesets and local Git publication. Publishing source does not authorize distribution or release automation.
