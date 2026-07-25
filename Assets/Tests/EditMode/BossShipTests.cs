using NUnit.Framework;
using UnityEngine;

public class BossShipTests
{
    [Test]
    public void CalculateNextPosition_MovesInConfiguredDirection()
    {
        GameObject bossObject = new GameObject("boss");
        BossShip boss = bossObject.AddComponent<BossShip>();
        boss.movementDirections = new[] { Vector2.right };
        boss.moveSpeed = 2f;
        boss.directionChangeInterval = 99f;
        boss.minBounds = new Vector2(-10f, -10f);
        boss.maxBounds = new Vector2(10f, 10f);

        Vector3 nextPosition = boss.CalculateNextPosition(Vector3.zero, 1f);

        Assert.AreEqual(2f, nextPosition.x, 0.001f);
        Assert.AreEqual(0f, nextPosition.y, 0.001f);
        Object.DestroyImmediate(bossObject);
    }

    [Test]
    public void CalculateNextPosition_ClampsToBounds()
    {
        GameObject bossObject = new GameObject("boss");
        BossShip boss = bossObject.AddComponent<BossShip>();
        boss.movementDirections = new[] { Vector2.right };
        boss.moveSpeed = 20f;
        boss.directionChangeInterval = 99f;
        boss.minBounds = new Vector2(-1f, -1f);
        boss.maxBounds = new Vector2(1f, 1f);

        Vector3 nextPosition = boss.CalculateNextPosition(Vector3.zero, 1f);

        Assert.AreEqual(1f, nextPosition.x, 0.001f);
        Assert.AreEqual(0f, nextPosition.y, 0.001f);
        Object.DestroyImmediate(bossObject);
    }
}
