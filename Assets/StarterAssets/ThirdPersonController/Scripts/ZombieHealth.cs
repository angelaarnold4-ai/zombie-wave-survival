using UnityEngine;

public class ZombieHealth : MonoBehaviour
{
    public float health = 50f;
    public AudioClip hurtSound;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        Debug.Log("Zombie hit! Current health: " + health);

        if (hurtSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(hurtSound);
        }

        if (health <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
        ScoreManager.instance.AddKill();
    }
}