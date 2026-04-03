using UnityEngine;

public class ZombieHealth : MonoBehaviour
{
    public float health = 50f;

    public void TakeDamage(float amount)
    {
        health -= amount;
        Debug.Log("Zombie hit! Current health: " + health);

        if (health <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        // For now, just destroy the zombie. 
        // Later you can add animations here!
        Destroy(gameObject);
    }
}