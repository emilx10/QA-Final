using NUnit.Framework;
using UnityEngine;

public class PlayerBoundsTests
{
    [Test]
    public void ClampPosition_KeepsPlayerInsideCalculatedBounds()
    {
        GameObject cameraObject = new GameObject("camera");
        Camera camera = cameraObject.AddComponent<Camera>();
        camera.orthographic = true;
        camera.orthographicSize = 5f;
        cameraObject.tag = "MainCamera";

        GameObject playerObject = new GameObject("player");
        PlayerMoving playerMoving = playerObject.AddComponent<PlayerMoving>();
        playerMoving.borders = new Borders
        {
            minXOffset = 1f,
            maxXOffset = 1f,
            minYOffset = 1f,
            maxYOffset = 1f
        };

        playerMoving.ResizeBorders();
        Vector3 clampedPosition = playerMoving.ClampPosition(new Vector3(999f, -999f, 0f));

        Assert.LessOrEqual(clampedPosition.x, playerMoving.borders.maxX);
        Assert.GreaterOrEqual(clampedPosition.y, playerMoving.borders.minY);

        Object.DestroyImmediate(playerObject);
        Object.DestroyImmediate(cameraObject);
    }
}
