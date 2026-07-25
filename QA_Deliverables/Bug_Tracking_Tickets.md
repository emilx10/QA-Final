# Online Ticket Backlog

These tickets are written in an online-tracker-ready format for Jira, GitHub Issues, Azure Boards, or Trello.

## QA-001 Add Automated Tests For Existing Combat Logic
- Type: Task
- Priority: High
- Status: Done
- Acceptance Criteria: EditMode tests cover damage, shield behavior, level difficulty, wave configuration, and boss movement.

## QA-002 Add Shield Defense To Enemies
- Type: Story
- Priority: High
- Status: Done
- Acceptance Criteria: Enemy shield absorbs damage before health; negative shield values are clamped; tests pass.

## QA-003 Add Boss Ship
- Type: Story
- Priority: High
- Status: Done
- Acceptance Criteria: Boss has large HP/shield capability, moves in varied directions, remains bounded, and has automated movement tests.

## QA-004 Expand Game To Two Levels
- Type: Story
- Priority: High
- Status: Done
- Acceptance Criteria: Level 1 and Level 2 settings exist; Level 2 increases health, shield, speed, and shot chance; tests pass.

## QA-005 Create CI/CD Pipeline
- Type: DevOps
- Priority: High
- Status: Done
- Acceptance Criteria: GitHub Actions workflow runs Unity EditMode tests and uploads results.

## QA-006 Cross-Platform Testing
- Type: QA
- Priority: Medium
- Status: Ready For Manual Execution
- Acceptance Criteria: WebGL and mobile smoke test evidence is attached.

## QA-007 Accessibility Testing
- Type: QA
- Priority: Medium
- Status: Ready For Manual Execution
- Acceptance Criteria: Input, visual clarity, readability, and motion checks are completed.
