using NUnit.Framework;
using UnityEngine;

public class WaveTests
{
    [Test]
    public void ConfigureEnemy_AppliesWaveDifficultySettings()
    {
        GameObject waveObject = new GameObject("wave");
        Wave wave = waveObject.AddComponent<Wave>();
        wave.enemyHealthMultiplier = 3;
        wave.enemyShieldPoints = 4;
        wave.shooting = new Shooting
        {
            shotChance = 55,
            shotTimeMin = 0.5f,
            shotTimeMax = 2f
        };

        GameObject enemyObject = new GameObject("enemy");
        Enemy enemy = enemyObject.AddComponent<Enemy>();
        enemy.health = 2;

        wave.ConfigureEnemy(enemyObject);

        Assert.AreEqual(6, enemy.health);
        Assert.AreEqual(4, enemy.shield);
        Assert.AreEqual(55, enemy.shotChance);
        Assert.AreEqual(0.5f, enemy.shotTimeMin);
        Assert.AreEqual(2f, enemy.shotTimeMax);

        Object.DestroyImmediate(enemyObject);
        Object.DestroyImmediate(waveObject);
    }
}
