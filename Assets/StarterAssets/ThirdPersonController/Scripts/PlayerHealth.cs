using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; 

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;
    public HealthBar healthBar;

    [Header("Audio")]
    public AudioClip ouchSound;
    void Start()
    {
        currentHealth = maxHealth;
        // Initialize the health bar at start
        healthBar.SetMaxHealth(maxHealth);
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;

        if (ouchSound != null)
        {
            // Plays the sound exactly where the player is standing
            AudioSource.PlayClipAtPoint(ouchSound, transform.position);
        }
        
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