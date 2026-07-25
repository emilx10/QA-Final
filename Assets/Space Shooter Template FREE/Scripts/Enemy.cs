using UnityEngine;

/// <summary>
/// Defines enemy health, shield defense, shooting, collision damage, and destruction.
/// </summary>
public class Enemy : MonoBehaviour
{
    #region FIELDS
    [Tooltip("Health points in integer")]
    public int health;

    [Tooltip("Shield points that absorb damage before health is reduced")]
    public int shield;

    [Tooltip("Enemy's projectile prefab")]
    public GameObject Projectile;

    [Tooltip("VFX prefab generating after destruction")]
    public GameObject destructionVFX;
    public GameObject hitEffect;

    [HideInInspector] public int shotChance;
    [HideInInspector] public float shotTimeMin, shotTimeMax;
    #endregion

    public bool ShieldActive
    {
        get { return shield > 0; }
    }

    private void Start()
    {
        if (Projectile != null && shotTimeMax >= shotTimeMin)
        {
            Invoke("ActivateShooting", Random.Range(shotTimeMin, shotTimeMax));
        }
    }

    void ActivateShooting()
    {
        if (Projectile != null && Random.value < (float)shotChance / 100)
        {
            Instantiate(Projectile, transform.position, Quaternion.identity);
        }
    }

    public void ConfigureShield(int shieldPoints)
    {
        shield = Mathf.Max(0, shieldPoints);
    }

    public void GetDamage(int damage)
    {
        if (damage <= 0)
        {
            return;
        }

        int remainingDamage = damage;
        if (shield > 0)
        {
            int absorbedDamage = Mathf.Min(shield, remainingDamage);
            shield -= absorbedDamage;
            remainingDamage -= absorbedDamage;
        }

        if (remainingDamage > 0)
        {
            health -= remainingDamage;
        }

        if (health <= 0)
        {
            Destruction();
        }
        else
        {
            SpawnHitEffect();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && Player.instance != null)
        {
            Projectile projectileComponent = Projectile != null ? Projectile.GetComponent<Projectile>() : null;
            Player.instance.GetDamage(projectileComponent != null ? projectileComponent.damage : 1);
        }
    }

    void SpawnHitEffect()
    {
        if (hitEffect != null)
        {
            Instantiate(hitEffect, transform.position, Quaternion.identity, transform);
        }
    }

    protected virtual void Destruction()
    {
        if (destructionVFX != null)
        {
            Instantiate(destructionVFX, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}
