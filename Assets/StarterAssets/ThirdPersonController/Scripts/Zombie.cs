using UnityEngine;

public class Zombie : MonoBehaviour
{
    [Header("Health Settings")]
    public float currentHealth = 100f;

    // Public so the PlayerShoot script can see it
    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        Debug.Log("Zombie took damage! Health left: " + currentHealth);

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
    // Find the WaveManager. Tell it a zombie died
    FindAnyObjectByType<WaveManager>().ZombieDied();
    Destroy(gameObject);
    }
}