using UnityEngine;

public class ZombieAttack : MonoBehaviour
{
    public float damageAmount = 10f;
    public float attackSpeed = 1.5f;
    private float nextAttackTime;

    // Handles physical bumping
    private void OnCollisionStay(Collision collision)
    {
        HandleDamage(collision.gameObject);
    }

    // Handles walking "through" the player if trigger is on
    private void OnTriggerStay(Collider other)
    {
        HandleDamage(other.gameObject);
    }

    void HandleDamage(GameObject target)
    {
        if (target.CompareTag("Player"))
        {
            if (Time.time >= nextAttackTime)
            {
                PlayerHealth health = target.GetComponent<PlayerHealth>();
                if (health != null)
                {
                    health.TakeDamage(damageAmount);
                    nextAttackTime = Time.time + attackSpeed;
                    Debug.Log("OUCH! Zombie hit you for " + damageAmount);
                }
            }
        }
    }
}