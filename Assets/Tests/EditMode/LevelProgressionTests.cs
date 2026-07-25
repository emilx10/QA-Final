using NUnit.Framework;
using UnityEngine;

public class LevelProgressionTests
{
    [Test]
    public void DefaultProgression_HasTwoLevelsAndStartsAtLevelOne()
    {
        GameObject controllerObject = new GameObject("level-progression");
        LevelProgression progression = controllerObject.AddComponent<LevelProgression>();

        Assert.AreEqual(1, progression.CurrentLevelNumber);
        Assert.IsTrue(progression.TryAdvanceLevel());
        Assert.AreEqual(2, progression.CurrentLevelNumber);
        Assert.IsFalse(progression.TryAdvanceLevel());

        Object.DestroyImmediate(controllerObject);
    }

    [Test]
    public void ApplyToEnemy_LevelTwoAddsHealthShieldAndShotChance()
    {
        GameObject controllerObject = new GameObject("level-progression");
        LevelProgression progression = controllerObject.AddComponent<LevelProgression>();
        progression.TryAdvanceLevel();

        GameObject enemyObject = new GameObject("enemy");
        Enemy enemy = enemyObject.AddComponent<Enemy>();
        enemy.health = 4;
        enemy.shotChance = 70;

        progression.ApplyToEnemy(enemy);

        Assert.AreEqual(8, enemy.health);
        Assert.AreEqual(3, enemy.shield);
        Assert.AreEqual(90, enemy.shotChance);

        Object.DestroyImmediate(enemyObject);
        Object.DestroyImmediate(controllerObject);
    }

    [Test]
    public void ApplyToWave_LevelTwoMakesWaveHarder()
    {
        GameObject controllerObject = new GameObject("level-progression");
        LevelProgression progression = controllerObject.AddComponent<LevelProgression>();
        progression.TryAdvanceLevel();

        GameObject waveObject = new GameObject("wave");
        Wave wave = waveObject.AddComponent<Wave>();
        wave.speed = 10f;
        wave.shooting = new Shooting { shotChance = 65 };

        progression.ApplyToWave(wave);

        Assert.AreEqual(13.5f, wave.speed, 0.001f);
        Assert.AreEqual(85, wave.shooting.shotChance);
        Assert.AreEqual(2, wave.enemyHealthMultiplier);
        Assert.AreEqual(3, wave.enemyShieldPoints);

        Object.DestroyImmediate(waveObject);
        Object.DestroyImmediate(controllerObject);
    }

    [Test]
    public void SetLevelIndex_CanStartDirectlyAtLevelTwo()
    {
        GameObject controllerObject = new GameObject("level-progression");
        LevelProgression progression = controllerObject.AddComponent<LevelProgression>();

        progression.SetLevelIndex(1);

        Assert.AreEqual(2, progression.CurrentLevelNumber);
        Assert.IsTrue(progression.CurrentLevel.spawnBoss);

        Object.DestroyImmediate(controllerObject);
    }
}
