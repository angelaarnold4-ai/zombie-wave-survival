using UnityEngine;
using TMPro; 
using UnityEngine.SceneManagement; // This allows the restart logic

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    public TextMeshProUGUI killText; 
    public TextMeshProUGUI waveText;
    
    private int totalKills = 0;

    void Awake()
    {
        instance = this;
    }

    public void AddKill()
    {
        totalKills++;
        if(killText != null) 
            killText.text = "Kills: " + totalKills;
    }

    public void UpdateWaveUI(int waveNumber)
    {
        if(waveText != null) 
            waveText.text = "Wave: " + waveNumber;
    }

    // Called from Player script when health <= 0
    public void TriggerGameOver()
    {
        Debug.Log("Player Died. Restarting Level...");
        // Reloads the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}