# Validation Report

Date: 2026-07-12

## Completed Checks
- Read assignment brief from `C:\Users\Emil\Desktop\Final Assignment.docx`.
- Implemented shielded enemies, boss ship, two-level progression, automated tests, QA deliverables, and CI/CD workflow.
- Verified the target Unity project is the running project at `D:\Games\QA - Final\QA - Final`.
- Ran C# solution build:
  - Command: `dotnet build "QA - Final.sln"`
  - Result: Passed, 0 warnings, 0 errors.
- Ran direct compile check for runtime scripts plus EditMode tests:
  - Command: `dotnet build assignment_staging\UnityAssignmentCompileCheck.csproj`
  - Result: Passed, 0 warnings, 0 errors.
- Verified `Boss_ship.prefab` references `BossShip`, has `health: 40`, and has `shield: 12`.
- Verified `Game_Controller.prefab` references `Boss_ship.prefab` for the level 2 boss.

## Blocked Check
- Unity EditMode test runner could not run while the same project was already open in the Unity Editor.
- Unity log message: `Multiple Unity instances cannot open the same project.`
- To run the tests, close the open Unity Editor instance and run the CI workflow or Unity Test Runner EditMode tests.

## Expected Automated Test Count
- 9 EditMode tests:
  - 3 enemy shield tests.
  - 3 level progression tests.
  - 2 boss movement tests.
  - 1 wave difficulty test.
