# Non-Functional Testing Summary

## Performance
- Run level 2 for 5 minutes in the Unity Editor.
- Watch for frame spikes, excessive allocations, and console errors.
- Verify projectiles, VFX, and enemy waves clean up correctly.

## Reliability
- Restart the scene 10 times.
- Confirm no missing references, singleton conflicts, or broken wave state.
- Confirm repeated damage and destruction events do not throw null reference errors.

## Compatibility
- Validate Windows Editor play mode.
- Build WebGL and confirm load/input smoke test.
- Build mobile target and confirm launch/input smoke test.

## Maintainability
- Core new behavior is covered by EditMode tests.
- Level configuration is isolated in `LevelSettings` and `LevelProgression`.
- Boss movement calculation is testable without scene dependencies.
