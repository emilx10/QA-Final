using UnityEngine;

/// <summary>
/// Applies two-level difficulty rules to enemies and waves.
/// </summary>
public class LevelProgression : MonoBehaviour
{
    public LevelSettings[] levels =
    {
        LevelSettings.CreateLevelOne(),
        LevelSettings.CreateLevelTwo()
    };

    [SerializeField] private int currentLevelIndex;

    public int CurrentLevelNumber
    {
        get { return CurrentLevel.levelNumber; }
    }

    public LevelSettings CurrentLevel
    {
        get
        {
            EnsureDefaultLevels();
            currentLevelIndex = Mathf.Clamp(currentLevelIndex, 0, levels.Length - 1);
            return levels[currentLevelIndex];
        }
    }

    public void SetLevelIndex(int levelIndex)
    {
        EnsureDefaultLevels();
        currentLevelIndex = Mathf.Clamp(levelIndex, 0, levels.Length - 1);
    }

    public bool TryAdvanceLevel()
    {
        EnsureDefaultLevels();
        if (currentLevelIndex >= levels.Length - 1)
        {
            return false;
        }

        currentLevelIndex++;
        return true;
    }

    public void ApplyToEnemy(Enemy enemy)
    {
        if (enemy == null)
        {
            return;
        }

        LevelSettings level = CurrentLevel;
        enemy.health = Mathf.Max(1, enemy.health * level.enemyHealthMultiplier);
        enemy.ConfigureShield(level.enemyShieldPoints);
        enemy.shotChance = Mathf.Clamp(enemy.shotChance + level.additionalShotChance, 0, 100);
    }

    public void ApplyToWave(Wave wave)
    {
        if (wave == null)
        {
            return;
        }

        LevelSettings level = CurrentLevel;
        wave.speed *= level.waveSpeedMultiplier;
        wave.shooting.shotChance = Mathf.Clamp(wave.shooting.shotChance + level.additionalShotChance, 0, 100);
        wave.enemyHealthMultiplier = level.enemyHealthMultiplier;
        wave.enemyShieldPoints = level.enemyShieldPoints;
    }

    private void EnsureDefaultLevels()
    {
        if (levels == null || levels.Length < 2)
        {
            levels = new[]
            {
                LevelSettings.CreateLevelOne(),
                LevelSettings.CreateLevelTwo()
            };
        }
    }
}
