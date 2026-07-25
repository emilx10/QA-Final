# Space Shooter Test Plan (STP)

## Scope
Validate the Space Shooter mini project after adding shielded enemies, a boss ship, two-level progression, automated tests, CI/CD, cross-platform checks, and accessibility checks.

## Test Objectives
- Confirm existing player, enemy, projectile, wave, and movement behavior still works.
- Confirm shielded enemies absorb damage before health is reduced.
- Confirm level 2 is harder than level 1 through enemy health, shields, speed, and shot chance.
- Confirm the boss has high HP, shield defense, and multi-direction bounded movement.
- Confirm automated EditMode tests run in CI.
- Confirm WebGL and mobile builds are considered in cross-platform test coverage.
- Confirm accessibility risks are documented and manually checked.

## Test Levels
- Unit/EditMode: shield damage, boss movement, level settings, wave difficulty application.
- Integration: enemy waves spawn with level settings and configured shooting values.
- Functional manual: complete level 1, advance to level 2, defeat shielded enemies, defeat boss.
- Non-functional: build stability, performance smoke checks, responsiveness, input usability.
- Cross-platform: Windows Editor, WebGL build, Android/iOS mobile layout and input smoke tests.

## Entry Criteria
- Unity project opens without compiler errors.
- Test Framework package is installed.
- All modified scripts are imported by Unity.
- CI workflow has Unity license secrets configured when run remotely.

## Exit Criteria
- All automated tests pass.
- No blocker or critical defects remain open.
- Manual smoke tests pass on target platforms or have documented issues.
- STP, STD, CI/CD, bug tracking, functional, non-functional, accessibility, and cross-platform evidence exists in the project.

## Risks
- Existing template scene references may need manual scene wiring for final gameplay balancing.
- Unity license secrets are required for GitHub Actions execution.
- Mobile controls may require additional UI tuning beyond script-level behavior.

## Responsibilities
- Developer: implement features, maintain automated tests, keep CI green.
- QA: run manual tests, log tickets, validate fixed defects.
- Release owner: confirm cross-platform build readiness.
