using System;
using UnityEngine;

[Serializable]
public class LevelSettings
{
    [Min(1)] public int levelNumber = 1;
    [Min(1)] public int enemyHealthMultiplier = 1;
    [Min(0)] public int enemyShieldPoints;
    [Range(0, 100)] public int additionalShotChance;
    [Min(0.1f)] public float waveSpeedMultiplier = 1f;
    public bool spawnBoss;

    public static LevelSettings CreateLevelOne()
    {
        return new LevelSettings
        {
            levelNumber = 1,
            enemyHealthMultiplier = 1,
            enemyShieldPoints = 0,
            additionalShotChance = 0,
            waveSpeedMultiplier = 1f,
            spawnBoss = false
        };
    }

    public static LevelSettings CreateLevelTwo()
    {
        return new LevelSettings
        {
            levelNumber = 2,
            enemyHealthMultiplier = 2,
            enemyShieldPoints = 3,
            additionalShotChance = 20,
            waveSpeedMultiplier = 1.35f,
            spawnBoss = true
        };
    }
}
