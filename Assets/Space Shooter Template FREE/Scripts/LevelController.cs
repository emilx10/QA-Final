using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

#region Serializable classes
[System.Serializable]
public class EnemyWaves
{
    [Tooltip("Time for wave generation from the moment the level started")]
    public float timeToStart;

    [Tooltip("Enemy wave's prefab")]
    public GameObject wave;
}
#endregion

public class LevelController : MonoBehaviour
{
    public EnemyWaves[] enemyWaves;

    [Header("Level Progression")]
    public LevelProgression levelProgression;
    public int startingLevelIndex;
    public string levelTwoSceneName = "Level_2";
    public GameObject bossPrefab;
    public float levelOneBossDelay = 20f;
    public float levelTwoBossDelay = 10f;
    public Vector2 bossSpawnPosition = new Vector2(0f, 4.2f);
    public int levelOneBossHealth = 45;
    public int levelOneBossShield = 10;
    public Vector3 levelOneBossScale = new Vector3(1.4f, 1.4f, 1f);
    public int levelTwoBossHealth = 120;
    public int levelTwoBossShield = 30;
    public Vector3 levelTwoBossScale = new Vector3(2.2f, 2.2f, 1f);

    public GameObject powerUp;
    public float timeForNewPowerup;
    public GameObject[] planets;
    public float timeBetweenPlanets;
    public float planetsSpeed;
    List<GameObject> planetsList = new List<GameObject>();

    Camera mainCamera;
    string statusText;
    bool bossSpawned;
    bool bossDefeated;

    private void Start()
    {
        mainCamera = Camera.main;
        EnsureLevelProgression();
        ApplySceneLevelOverride();
        levelProgression.SetLevelIndex(startingLevelIndex);

        statusText = levelProgression.CurrentLevelNumber == 1
            ? "LEVEL 1 - Survive the first wave"
            : "LEVEL 2 - Shielded enemies incoming";

        StartCurrentLevelWaves();
        StartCoroutine(PowerupBonusCreation());
        StartCoroutine(PlanetsCreation());
        StartCoroutine(RunSceneFlow());
    }

    IEnumerator RunSceneFlow()
    {
        if (levelProgression.CurrentLevelNumber == 1)
        {
            statusText = "LEVEL 1 - Boss arrives in " + Mathf.CeilToInt(levelOneBossDelay) + " seconds";
            yield return new WaitForSeconds(levelOneBossDelay);
            SpawnBoss();
        }
        else
        {
            statusText = "LEVEL 2 - Boss arrives in " + Mathf.CeilToInt(levelTwoBossDelay) + " seconds";
            yield return new WaitForSeconds(levelTwoBossDelay);
            SpawnBoss();
        }
    }

    void StartCurrentLevelWaves()
    {
        for (int i = 0; i < enemyWaves.Length; i++)
        {
            StartCoroutine(CreateEnemyWave(enemyWaves[i].timeToStart, enemyWaves[i].wave));
        }
    }

    IEnumerator CreateEnemyWave(float delay, GameObject wave)
    {
        if (delay != 0)
        {
            yield return new WaitForSeconds(delay);
        }

        if (Player.instance != null && wave != null)
        {
            GameObject waveInstance = Instantiate(wave);
            Wave waveComponent = waveInstance.GetComponent<Wave>();
            levelProgression.ApplyToWave(waveComponent);
        }
    }

    void SpawnBoss()
    {
        if (bossSpawned || bossPrefab == null || Player.instance == null)
        {
            return;
        }

        bossSpawned = true;
        bool isLevelTwo = levelProgression.CurrentLevelNumber == 2;
        statusText = isLevelTwo
            ? "LEVEL 2 BOSS - Bigger ship, heavier shield"
            : "LEVEL 1 BOSS - Break the shield, then the hull";

        GameObject boss = Instantiate(bossPrefab, bossSpawnPosition, Quaternion.identity);
        boss.transform.localScale = isLevelTwo ? levelTwoBossScale : levelOneBossScale;

        BossShip bossShip = boss.GetComponent<BossShip>();
        if (bossShip != null)
        {
            bossShip.health = isLevelTwo ? levelTwoBossHealth : levelOneBossHealth;
            bossShip.ConfigureShield(isLevelTwo ? levelTwoBossShield : levelOneBossShield);
            bossShip.moveSpeed = isLevelTwo ? 3.6f : 2.5f;
            bossShip.shotChance = isLevelTwo ? 95 : 70;
        }
    }

    public void RegisterBossDefeated()
    {
        if (!bossSpawned || bossDefeated)
        {
            return;
        }

        bossDefeated = true;
        if (levelProgression.CurrentLevelNumber == 1)
        {
            statusText = "LEVEL 1 CLEARED - Loading Level 2";
            StartCoroutine(LoadLevelTwoAfterDelay());
        }
        else
        {
            statusText = "MISSION COMPLETE - Assignment objectives cleared";
        }
    }

    IEnumerator LoadLevelTwoAfterDelay()
    {
        yield return new WaitForSeconds(2f);
        if (!string.IsNullOrEmpty(levelTwoSceneName))
        {
            SceneManager.LoadScene(levelTwoSceneName);
        }
    }

    IEnumerator PowerupBonusCreation()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeForNewPowerup);
            if (PlayerMoving.instance == null || mainCamera == null || powerUp == null)
            {
                continue;
            }

            Vector2 powerUpPadding = GetPowerUpHalfExtents();
            Vector2 spawnPosition = PlayerMoving.instance.GetRandomPlayablePosition(powerUpPadding.x, powerUpPadding.y);
            Instantiate(
                powerUp,
                spawnPosition,
                Quaternion.identity
            );
        }
    }

    Vector2 GetPowerUpHalfExtents()
    {
        Renderer renderer = powerUp != null ? powerUp.GetComponentInChildren<Renderer>() : null;
        if (renderer == null)
        {
            return Vector2.zero;
        }

        return new Vector2(renderer.bounds.extents.x, renderer.bounds.extents.y);
    }

    IEnumerator PlanetsCreation()
    {
        for (int i = 0; i < planets.Length; i++)
        {
            planetsList.Add(planets[i]);
        }

        yield return new WaitForSeconds(10);
        while (true)
        {
            if (planetsList.Count == 0)
            {
                for (int i = 0; i < planets.Length; i++)
                {
                    planetsList.Add(planets[i]);
                }
            }

            if (planetsList.Count > 0)
            {
                int randomIndex = Random.Range(0, planetsList.Count);
                GameObject newPlanet = Instantiate(planetsList[randomIndex]);
                planetsList.RemoveAt(randomIndex);
                DirectMoving directMoving = newPlanet.GetComponent<DirectMoving>();
                if (directMoving != null)
                {
                    directMoving.speed = planetsSpeed;
                }
            }

            yield return new WaitForSeconds(timeBetweenPlanets);
        }
    }

    void EnsureLevelProgression()
    {
        if (levelProgression == null)
        {
            levelProgression = GetComponent<LevelProgression>();
        }

        if (levelProgression == null)
        {
            levelProgression = gameObject.AddComponent<LevelProgression>();
        }
    }

    void ApplySceneLevelOverride()
    {
        if (SceneManager.GetActiveScene().name == levelTwoSceneName)
        {
            startingLevelIndex = 1;
        }
    }

    private void OnGUI()
    {
        GUIStyle style = new GUIStyle(GUI.skin.label)
        {
            fontSize = 24,
            fontStyle = FontStyle.Bold,
            alignment = TextAnchor.UpperCenter
        };
        style.normal.textColor = Color.white;
        GUI.Label(new Rect(0, 14, Screen.width, 40), statusText, style);
    }
}
