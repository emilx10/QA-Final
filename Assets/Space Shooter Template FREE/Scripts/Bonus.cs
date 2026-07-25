using UnityEngine;

public class Bonus : MonoBehaviour
{
    Renderer bonusRenderer;

    private void Awake()
    {
        bonusRenderer = GetComponentInChildren<Renderer>();
    }

    private void LateUpdate()
    {
        if (PlayerMoving.instance == null)
        {
            return;
        }

        Vector2 padding = GetHalfExtents();
        transform.position = PlayerMoving.instance.ClampPosition(transform.position, padding.x, padding.y);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (PlayerShooting.instance.weaponPower < PlayerShooting.instance.maxweaponPower)
            {
                PlayerShooting.instance.weaponPower++;
            }

            Destroy(gameObject);
        }
    }

    Vector2 GetHalfExtents()
    {
        if (bonusRenderer == null)
        {
            bonusRenderer = GetComponentInChildren<Renderer>();
        }

        if (bonusRenderer == null)
        {
            return Vector2.zero;
        }

        Bounds bounds = bonusRenderer.bounds;
        return new Vector2(bounds.extents.x, bounds.extents.y);
    }
}
