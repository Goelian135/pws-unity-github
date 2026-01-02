using UnityEngine;

public class WeaponHitbox : MonoBehaviour
{
    [SerializeField] private int damage = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Probeer een health / damage component te vinden
        if (other.TryGetComponent<EnemyController>(out EnemyController health))
        {
            health.TakeDamage(damage);
        }

        if (other.TryGetComponent<BossController>(out BossController bossHealth))
        {
            bossHealth.TakeDamage(damage);
        }
    }
}
