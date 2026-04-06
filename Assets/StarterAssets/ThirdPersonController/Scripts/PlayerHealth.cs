using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;

    // Reference to the health bar script
    public HealthBar healthBar;

    void Start()
    {
        currentHealth = maxHealth;
        // Initialize the health bar at start
        healthBar.SetMaxHealth(maxHealth);
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;

        // Update UI health bar
        healthBar.UpdateHealth(currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Update()
    {
        if (currentHealth <= 0)
        {
        Die(); 
        }
    }

    void GameOver()
    {
    Debug.Log("GAME OVER");
    
    }

    void Die()
    {
        // Tells ScoreManager to reset the game
        ScoreManager.instance.TriggerGameOver();
        
    }
}