using UnityEngine;

/// <summary>
/// Defines and enforces the player's movement borders for desktop and mobile input.
/// The playable area follows the visible background bounds when a background renderer exists.
/// </summary>
[System.Serializable]
public class Borders
{
    [Tooltip("Offset from viewport/background borders for player's movement")]
    public float minXOffset = 0f, maxXOffset = 0f, minYOffset = 0f, maxYOffset = 0f;
    [HideInInspector] public float minX, maxX, minY, maxY;
}

public class PlayerMoving : MonoBehaviour
{
    [Tooltip("Offset from viewport/background borders for player's movement")]
    public Borders borders;

    [Tooltip("Use the rendered background rectangle as the movement boundary when available.")]
    public bool useBackgroundAsBoundary = true;

    [Header("Keyboard Movement")]
    [Tooltip("Allow the player to move with the W, A, S, and D keys on desktop.")]
    public bool enableWasdMovement = true;

    [Min(0f)]
    [Tooltip("Player movement speed in world units per second when using WASD.")]
    public float keyboardMoveSpeed = 10f;

    Camera mainCamera;
    Renderer playerRenderer;
    Vector2 lastScreenSize;
    Bounds? cachedBackgroundBounds;
    bool controlIsActive = true;

    public static PlayerMoving instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        mainCamera = Camera.main;
        playerRenderer = GetComponentInChildren<Renderer>();
        ResizeBorders();
    }

    private void Update()
    {
        RefreshBordersIfScreenChanged();

        if (controlIsActive)
        {
#if UNITY_STANDALONE || UNITY_EDITOR
            Vector2 keyboardDirection = GetWasdDirection();
            if (enableWasdMovement && keyboardDirection.sqrMagnitude > 0f)
            {
                transform.position += (Vector3)(keyboardDirection * keyboardMoveSpeed * Time.deltaTime);
            }
            else if (Input.GetMouseButton(0) && mainCamera != null)
            {
                Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                mousePosition.z = transform.position.z;
                transform.position = Vector3.MoveTowards(transform.position, mousePosition, 30 * Time.deltaTime);
            }
#endif

#if UNITY_IOS || UNITY_ANDROID
            if (Input.touchCount == 1 && mainCamera != null)
            {
                Touch touch = Input.touches[0];
                Vector3 touchPosition = mainCamera.ScreenToWorldPoint(touch.position);
                touchPosition.z = transform.position.z;
                transform.position = Vector3.MoveTowards(transform.position, touchPosition, 30 * Time.deltaTime);
            }
#endif
        }

        transform.position = ClampPosition(transform.position);
    }

    Vector2 GetWasdDirection()
    {
        float horizontal = 0f;
        float vertical = 0f;

        if (Input.GetKey(KeyCode.A)) horizontal -= 1f;
        if (Input.GetKey(KeyCode.D)) horizontal += 1f;
        if (Input.GetKey(KeyCode.S)) vertical -= 1f;
        if (Input.GetKey(KeyCode.W)) vertical += 1f;

        return Vector2.ClampMagnitude(new Vector2(horizontal, vertical), 1f);
    }

    public Vector3 ClampPosition(Vector3 position)
    {
        return ClampPosition(position, 0f, 0f);
    }

    public Vector3 ClampPosition(Vector3 position, float extraXPadding, float extraYPadding)
    {
        ResizeBorders();
        float minX = Mathf.Min(borders.minX + extraXPadding, borders.maxX - extraXPadding);
        float maxX = Mathf.Max(borders.minX + extraXPadding, borders.maxX - extraXPadding);
        float minY = Mathf.Min(borders.minY + extraYPadding, borders.maxY - extraYPadding);
        float maxY = Mathf.Max(borders.minY + extraYPadding, borders.maxY - extraYPadding);

        position.x = Mathf.Clamp(position.x, minX, maxX);
        position.y = Mathf.Clamp(position.y, minY, maxY);
        position.z = transform.position.z;
        return position;
    }

    public Vector2 GetRandomPlayablePosition(float extraXPadding, float extraYPadding)
    {
        ResizeBorders();
        float minX = Mathf.Min(borders.minX + extraXPadding, borders.maxX - extraXPadding);
        float maxX = Mathf.Max(borders.minX + extraXPadding, borders.maxX - extraXPadding);
        float minY = Mathf.Min(borders.minY + extraYPadding, borders.maxY - extraYPadding);
        float maxY = Mathf.Max(borders.minY + extraYPadding, borders.maxY - extraYPadding);
        return new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
    }

    public void ResizeBorders()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        Bounds playableBounds;
        if (useBackgroundAsBoundary && TryGetBackgroundBounds(out playableBounds))
        {
            ApplyBounds(playableBounds);
            return;
        }

        if (mainCamera == null)
        {
            return;
        }

        Vector3 bottomLeft = mainCamera.ViewportToWorldPoint(Vector3.zero);
        Vector3 topRight = mainCamera.ViewportToWorldPoint(Vector3.one);
        playableBounds = new Bounds();
        playableBounds.SetMinMax(bottomLeft, topRight);
        ApplyBounds(playableBounds);
    }

    void ApplyBounds(Bounds playableBounds)
    {
        Vector2 halfExtents = GetPlayerHalfExtents();
        borders.minX = playableBounds.min.x + borders.minXOffset + halfExtents.x;
        borders.minY = playableBounds.min.y + borders.minYOffset + halfExtents.y;
        borders.maxX = playableBounds.max.x - borders.maxXOffset - halfExtents.x;
        borders.maxY = playableBounds.max.y - borders.maxYOffset - halfExtents.y;
        lastScreenSize = new Vector2(Screen.width, Screen.height);
    }

    bool TryGetBackgroundBounds(out Bounds backgroundBounds)
    {
        if (cachedBackgroundBounds.HasValue)
        {
            backgroundBounds = cachedBackgroundBounds.Value;
            return true;
        }

        Renderer[] renderers = FindObjectsByType<Renderer>(FindObjectsSortMode.None);
        bool foundBackground = false;
        backgroundBounds = new Bounds();

        for (int i = 0; i < renderers.Length; i++)
        {
            Renderer candidate = renderers[i];
            if (candidate == null || !candidate.enabled || !IsBackgroundRenderer(candidate))
            {
                continue;
            }

            if (!foundBackground)
            {
                backgroundBounds = candidate.bounds;
                foundBackground = true;
            }
            else
            {
                backgroundBounds.Encapsulate(candidate.bounds);
            }
        }

        if (foundBackground)
        {
            cachedBackgroundBounds = backgroundBounds;
        }

        return foundBackground;
    }

    bool IsBackgroundRenderer(Renderer renderer)
    {
        Transform current = renderer.transform;
        while (current != null)
        {
            string objectName = current.name.ToLowerInvariant();
            if (objectName.Contains("background") || objectName.Contains("nebula") || objectName.Contains("stars"))
            {
                return true;
            }

            current = current.parent;
        }

        return false;
    }

    void RefreshBordersIfScreenChanged()
    {
        if (lastScreenSize.x != Screen.width || lastScreenSize.y != Screen.height)
        {
            cachedBackgroundBounds = null;
            ResizeBorders();
        }
    }

    Vector2 GetPlayerHalfExtents()
    {
        if (playerRenderer == null)
        {
            playerRenderer = GetComponentInChildren<Renderer>();
        }

        if (playerRenderer == null)
        {
            return Vector2.zero;
        }

        Bounds bounds = playerRenderer.bounds;
        return new Vector2(bounds.extents.x, bounds.extents.y);
    }
}
