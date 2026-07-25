# Space Shooter Test Design (STD)

## Automated Tests

| ID | Area | Scenario | Expected Result |
| --- | --- | --- | --- |
| AT-001 | Enemy Shield | Damage is less than shield value | Shield decreases, health does not change |
| AT-002 | Enemy Shield | Damage exceeds shield value | Shield reaches 0, overflow damage reduces health |
| AT-003 | Enemy Shield | Negative shield configuration | Shield is clamped to 0 |
| AT-004 | Level Progression | Default progression advances once | Level 1 advances to level 2, then stops |
| AT-005 | Level Progression | Level 2 applied to enemy | Health doubles, shield is added, shot chance increases |
| AT-006 | Level Progression | Level 2 applied to wave | Wave speed, shot chance, health multiplier, and shield settings increase |
| AT-007 | Boss Movement | Boss moves in configured direction | Position changes by direction and speed |
| AT-008 | Boss Movement | Boss reaches bounds | Position is clamped inside configured bounds |
| AT-009 | Wave Difficulty | Wave configures spawned enemy | Enemy receives health multiplier, shield, and shooting values |

## Manual Functional Tests

| ID | Scenario | Steps | Expected Result |
| --- | --- | --- | --- |
| FT-001 | Start Game | Open game scene and press Play | Player ship appears and can move |
| FT-002 | Shoot Enemy | Fire at a normal enemy | Enemy takes health damage and is destroyed at 0 HP |
| FT-003 | Shielded Enemy | Fire at level 2 shielded enemy | Shield absorbs first damage before HP decreases |
| FT-004 | Level 2 Difficulty | Progress from level 1 to level 2 | Enemies are tougher, faster, and more aggressive |
| FT-005 | Boss Encounter | Reach level 2 boss | Boss appears, moves in multiple directions, has high HP, and can be defeated |
| FT-006 | Game Over | Collide with enemy/enemy projectile | Player destruction behavior triggers |

## Manual Non-Functional Tests

| ID | Scenario | Steps | Expected Result |
| --- | --- | --- | --- |
| NFT-001 | Editor Performance | Run level 2 for 5 minutes | No major frame drops, leaks, or console spam |
| NFT-002 | WebGL Build | Build and run WebGL | Game loads and accepts input |
| NFT-003 | Mobile Build | Build and run mobile target | Game launches, input remains usable, UI fits screen |
| NFT-004 | Stability | Restart scene repeatedly | No stuck singleton, missing reference, or persistent broken state |

## Accessibility Tests

| ID | Scenario | Steps | Expected Result |
| --- | --- | --- | --- |
| AX-001 | Keyboard Access | Play without mouse | Core movement and shooting are possible |
| AX-002 | Visual Clarity | Review player, bullets, enemies, background | Critical objects remain distinguishable |
| AX-003 | Motion Comfort | Review boss and background motion | No excessive flashing or unavoidable extreme motion |
| AX-004 | Text/UI | Review menu or HUD text if added | Text is readable and not clipped on target resolutions |
