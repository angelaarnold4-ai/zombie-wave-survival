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
    // This will print the name of EVERYTHING the zombie touches
    Debug.Log("Zombie touched: " + target.name + " with Tag: " + target.tag);

    if (target.CompareTag("Player"))
    {
        Debug.Log("Target IS the Player! Checking for Health Script...");
        PlayerHealth health = target.GetComponent<PlayerHealth>();
        
        if (health != null)
        {
            if (Time.time >= nextAttackTime)
            {
                health.TakeDamage(damageAmount);
                nextAttackTime = Time.time + attackSpeed;
                Debug.Log("SUCCESS: Damage Sent!");
            }
        }
        else
        {
            Debug.LogError("FAIL: Target has Player tag, but NO PlayerHealth script attached!");
        }
    }
}
}