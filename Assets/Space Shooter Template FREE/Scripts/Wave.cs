using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// This script generates an enemy wave. It defines how many enemies emerge, their speed,
/// emerging interval, shooting mode, moving path, and level-based difficulty overrides.
/// </summary>
[Serializable]
public class Shooting
{
    [Range(0, 100)]
    [Tooltip("Probability with which the ship of this wave will make a shot")]
    public int shotChance;

    [Tooltip("Min and max time from the beginning of the path when the enemy can make a shot")]
    public float shotTimeMin, shotTimeMax;
}

public class Wave : MonoBehaviour
{
    #region FIELDS
    [Tooltip("Enemy's prefab")]
    public GameObject enemy;

    [Tooltip("A number of enemies in the wave")]
    public int count;

    [Tooltip("Path passage speed")]
    public float speed;

    [Tooltip("Time between emerging of the enemies in the wave")]
    public float timeBetween;

    [Tooltip("Points of the path. Delete or add elements to the list if you want to change the number of the points")]
    public Transform[] pathPoints;

    [Tooltip("Whether enemy rotates in path passage direction")]
    public bool rotationByPath;

    [Tooltip("If loop is activated, after completing the path enemy will return to the starting point")]
    public bool Loop;

    [Tooltip("Color of the path in the Editor")]
    public Color pathColor = Color.yellow;
    public Shooting shooting;

    [Tooltip("If testMode is marked the wave will be re-generated after 3 sec")]
    public bool testMode;

    [Tooltip("Level-applied multiplier for spawned enemy health")]
    public int enemyHealthMultiplier = 1;

    [Tooltip("Level-applied shield points for spawned enemies")]
    public int enemyShieldPoints;
    #endregion

    private void Start()
    {
        StartCoroutine(CreateEnemyWave());
    }

    IEnumerator CreateEnemyWave()
    {
        for (int i = 0; i < count; i++)
        {
            GameObject newEnemy = Instantiate(enemy, enemy.transform.position, Quaternion.identity);
            ConfigureMovement(newEnemy);
            ConfigureEnemy(newEnemy);
            newEnemy.SetActive(true);
            yield return new WaitForSeconds(timeBetween);
        }

        if (testMode)
        {
            yield return new WaitForSeconds(3);
            StartCoroutine(CreateEnemyWave());
        }
        else if (!Loop)
        {
            Destroy(gameObject);
        }
    }

    public void ConfigureEnemy(GameObject enemyObject)
    {
        if (enemyObject == null)
        {
            return;
        }

        Enemy enemyComponent = enemyObject.GetComponent<Enemy>();
        if (enemyComponent == null)
        {
            return;
        }

        enemyComponent.health = Mathf.Max(1, enemyComponent.health * Mathf.Max(1, enemyHealthMultiplier));
        enemyComponent.ConfigureShield(enemyShieldPoints);
        enemyComponent.shotChance = shooting.shotChance;
        enemyComponent.shotTimeMin = shooting.shotTimeMin;
        enemyComponent.shotTimeMax = shooting.shotTimeMax;
    }

    private void ConfigureMovement(GameObject enemyObject)
    {
        FollowThePath followComponent = enemyObject.GetComponent<FollowThePath>();
        if (followComponent == null)
        {
            return;
        }

        followComponent.path = pathPoints;
        followComponent.speed = speed;
        followComponent.rotationByPath = rotationByPath;
        followComponent.loop = Loop;
        followComponent.SetPath();
    }

    void OnDrawGizmos()
    {
        if (pathPoints == null || pathPoints.Length < 2)
        {
            return;
        }

        DrawPath(pathPoints);
    }

    void DrawPath(Transform[] path)
    {
        Vector3[] pathPositions = new Vector3[path.Length];
        for (int i = 0; i < path.Length; i++)
        {
            pathPositions[i] = path[i].position;
        }

        Vector3[] newPathPositions = CreatePoints(pathPositions);
        Vector3 previosPositions = Interpolate(newPathPositions, 0);
        Gizmos.color = pathColor;
        int smoothAmount = path.Length * 20;
        for (int i = 1; i <= smoothAmount; i++)
        {
            float t = (float)i / smoothAmount;
            Vector3 currentPositions = Interpolate(newPathPositions, t);
            Gizmos.DrawLine(currentPositions, previosPositions);
            previosPositions = currentPositions;
        }
    }

    Vector3 Interpolate(Vector3[] path, float t)
    {
        int numSections = path.Length - 3;
        int currPt = Mathf.Min(Mathf.FloorToInt(t * numSections), numSections - 1);
        float u = t * numSections - currPt;
        Vector3 a = path[currPt];
        Vector3 b = path[currPt + 1];
        Vector3 c = path[currPt + 2];
        Vector3 d = path[currPt + 3];
        return 0.5f * ((-a + 3f * b - 3f * c + d) * (u * u * u) + (2f * a - 5f * b + 4f * c - d) * (u * u) + (-a + c) * u + 2f * b);
    }

    Vector3[] CreatePoints(Vector3[] path)
    {
        const int dist = 2;
        Vector3[] newPathPos = new Vector3[path.Length + dist];
        Array.Copy(path, 0, newPathPos, 1, path.Length);
        newPathPos[0] = newPathPos[1] + (newPathPos[1] - newPathPos[2]);
        newPathPos[newPathPos.Length - 1] = newPathPos[newPathPos.Length - 2] + (newPathPos[newPathPos.Length - 2] - newPathPos[newPathPos.Length - 3]);
        if (newPathPos[1] == newPathPos[newPathPos.Length - 2])
        {
            Vector3[] loopSpline = new Vector3[newPathPos.Length];
            Array.Copy(newPathPos, loopSpline, newPathPos.Length);
            loopSpline[0] = loopSpline[loopSpline.Length - 3];
            loopSpline[loopSpline.Length - 1] = loopSpline[2];
            newPathPos = new Vector3[loopSpline.Length];
            Array.Copy(loopSpline, newPathPos, loopSpline.Length);
        }

        return newPathPos;
    }
}
