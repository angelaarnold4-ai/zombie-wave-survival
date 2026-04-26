using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; 

public class PlayerHealth : MonoBehaviour
{
    [Header("UI References")]
    public Image redVignette; // Drag your Red Vignette Image here
    public HealthBar healthBar;

    [Header("Health Settings")]
    public float maxHealth = 100f;
    private float currentHealth; // Removed the duplicate line here

    [Header("Audio")]
    public AudioClip ouchSound;

    void Start()
    {
        currentHealth = maxHealth;
        
        // Initialize health bar
        if (healthBar != null)
            healthBar.SetMaxHealth(maxHealth);

        // Make sure vignette starts invisible
        if (redVignette != null)
            redVignette.color = new Color(1, 0, 0, 0);
    }

    void Update()
    {
        UpdateVignette();
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;

        if (ouchSound != null)
        {
            AudioSource.PlayClipAtPoint(ouchSound, transform.position);
        }
        
        if (healthBar != null)
            healthBar.UpdateHealth(currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // --- NEW: Logic for Task #6 ---
    void UpdateVignette()
    {
        if (redVignette == null) return;

        // Check if health is below 25%
        float threshold = maxHealth * 0.25f;

        if (currentHealth <= threshold && currentHealth > 0)
        {
            // This math makes it get redder as you get closer to 0
            // 0.5f is the "max" redness so the player can still see the game
            float alpha = 1f - (currentHealth / threshold);
            alpha = Mathf.Clamp(alpha, 0f, 0.5f); 

            redVignette.color = new Color(1, 0, 0, alpha);
        }
        else
        {
            // Stay invisible if health is high
            redVignette.color = new Color(1, 0, 0, 0);
        }
    }

    void Die()
    {
        // Tells ScoreManager to show Game Over screen and stop everything
        if (ScoreManager.instance != null)
        {
            ScoreManager.instance.TriggerGameOver();
        }
    }
}