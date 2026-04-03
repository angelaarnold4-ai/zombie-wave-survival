using UnityEngine;

public class Zombie : MonoBehaviour
{
    [Header("Health Settings")]
    public float currentHealth = 100f;

    // This function is "Public" so the PlayerShoot script can see it
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
        Debug.Log("Zombie has been destroyed!");
        // This removes the zombie from the game
        Destroy(gameObject);
    }
}