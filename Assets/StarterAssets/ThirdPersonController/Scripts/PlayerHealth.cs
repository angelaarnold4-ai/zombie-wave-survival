using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;

    // Reference to the health bar script
    public HealthBar healthBar;

    void Start()
    {
        currentHealth = maxHealth;
        // Initialize the health bar at the start
        healthBar.SetMaxHealth(maxHealth);
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;

        // Update the UI health bar
        healthBar.UpdateHealth(currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Game Over");
        // You could also disable movement or reload the scene here
    }
}