# Functional Testing Summary

Functional testing covers the player loop, enemy behavior, projectiles, waves, shields, two-level progression, and boss encounter.

## Covered Features
- Player movement and destruction.
- Projectile damage routing.
- Normal enemy health.
- Shielded enemy defense.
- Enemy shooting settings.
- Wave spawning and difficulty modifiers.
- Two-level progression rules.
- Boss high HP and multi-direction movement.

## Acceptance Criteria
- Level 1 remains playable with normal enemy behavior.
- Level 2 is measurably harder through health, shield, speed, and shot chance.
- Shielded enemies never lose health until shield damage is exhausted.
- Boss movement uses more than one direction and stays within bounds.
- Automated tests pass locally and in CI.
