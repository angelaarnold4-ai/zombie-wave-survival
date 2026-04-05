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
        // Destroys the zombie. 
        // Add animations here
        Destroy(gameObject);
    }
}