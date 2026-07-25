using UnityEngine;

/// <summary>
/// Boss enemy with large health and a reusable multi-direction movement pattern.
/// </summary>
public class BossShip : Enemy
{
    [Tooltip("Directions the boss cycles through while moving.")]
    public Vector2[] movementDirections =
    {
        Vector2.left,
        Vector2.right,
        Vector2.down,
        new Vector2(1f, -0.5f),
        new Vector2(-1f, -0.5f)
    };

    [Tooltip("Boss movement speed in world units per second.")]
    public float moveSpeed = 3f;

    [Tooltip("Seconds before switching to the next movement direction.")]
    public float directionChangeInterval = 1.5f;

    [Tooltip("Minimum world position allowed for the boss.")]
    public Vector2 minBounds = new Vector2(-7.5f, -3.5f);

    [Tooltip("Maximum world position allowed for the boss.")]
    public Vector2 maxBounds = new Vector2(7.5f, 4.5f);

    private int directionIndex;
    private float directionTimer;

    private void Reset()
    {
        health = 40;
        shield = 12;
        shotChance = 85;
        shotTimeMin = 0.3f;
        shotTimeMax = 1.2f;
    }

    private void Update()
    {
        transform.position = CalculateNextPosition(transform.position, Time.deltaTime);
    }

    public Vector3 CalculateNextPosition(Vector3 currentPosition, float deltaTime)
    {
        if (movementDirections == null || movementDirections.Length == 0 || moveSpeed <= 0f)
        {
            return currentPosition;
        }

        directionTimer += deltaTime;
        if (directionTimer >= directionChangeInterval)
        {
            directionTimer = 0f;
            directionIndex = (directionIndex + 1) % movementDirections.Length;
        }

        Vector2 direction = movementDirections[directionIndex].sqrMagnitude > 0f
            ? movementDirections[directionIndex].normalized
            : Vector2.zero;

        Vector3 nextPosition = currentPosition + (Vector3)(direction * moveSpeed * deltaTime);
        nextPosition.x = Mathf.Clamp(nextPosition.x, minBounds.x, maxBounds.x);
        nextPosition.y = Mathf.Clamp(nextPosition.y, minBounds.y, maxBounds.y);
        return nextPosition;
    }

    protected override void Destruction()
    {
        LevelController levelController = FindFirstObjectByType<LevelController>();
        if (levelController != null)
        {
            levelController.RegisterBossDefeated();
        }

        base.Destruction();
    }
}
