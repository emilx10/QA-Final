using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelControllerTests
{
    [Test]
    public void BossSettings_LevelTwoBossIsBiggerAndTougherThanLevelOneBoss()
    {
        GameObject controllerObject = new GameObject("level-controller");
        LevelController controller = controllerObject.AddComponent<LevelController>();

        Assert.Greater(controller.levelTwoBossHealth, controller.levelOneBossHealth);
        Assert.Greater(controller.levelTwoBossShield, controller.levelOneBossShield);
        Assert.Greater(controller.levelTwoBossScale.x, controller.levelOneBossScale.x);
        Assert.Greater(controller.levelTwoBossScale.y, controller.levelOneBossScale.y);

        Object.DestroyImmediate(controllerObject);
    }

    [Test]
    public void ApplySceneLevelOverride_LevelTwoSceneStartsAtSecondLevel()
    {
        GameObject controllerObject = new GameObject("level-controller");
        LevelController controller = controllerObject.AddComponent<LevelController>();
        controller.levelTwoSceneName = SceneManager.GetActiveScene().name;

        MethodInfo method = typeof(LevelController).GetMethod(
            "ApplySceneLevelOverride",
            BindingFlags.Instance | BindingFlags.NonPublic);
        method.Invoke(controller, null);

        Assert.AreEqual(1, controller.startingLevelIndex);

        Object.DestroyImmediate(controllerObject);
    }
}
