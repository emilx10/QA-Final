using NUnit.Framework;
using UnityEngine;

public class EnemyShieldTests
{
    [Test]
    public void GetDamage_ConsumesShieldBeforeHealth()
    {
        GameObject enemyObject = new GameObject("shielded-enemy");
        Enemy enemy = enemyObject.AddComponent<Enemy>();
        enemy.health = 10;
        enemy.ConfigureShield(5);

        enemy.GetDamage(3);

        Assert.AreEqual(2, enemy.shield);
        Assert.AreEqual(10, enemy.health);
        Object.DestroyImmediate(enemyObject);
    }

    [Test]
    public void GetDamage_OverflowDamageReducesHealth()
    {
        GameObject enemyObject = new GameObject("shielded-enemy");
        Enemy enemy = enemyObject.AddComponent<Enemy>();
        enemy.health = 10;
        enemy.ConfigureShield(2);

        enemy.GetDamage(5);

        Assert.AreEqual(0, enemy.shield);
        Assert.AreEqual(7, enemy.health);
        Object.DestroyImmediate(enemyObject);
    }

    [Test]
    public void ConfigureShield_RejectsNegativeValues()
    {
        GameObject enemyObject = new GameObject("shielded-enemy");
        Enemy enemy = enemyObject.AddComponent<Enemy>();

        enemy.ConfigureShield(-99);

        Assert.AreEqual(0, enemy.shield);
        Assert.IsFalse(enemy.ShieldActive);
        Object.DestroyImmediate(enemyObject);
    }
}
