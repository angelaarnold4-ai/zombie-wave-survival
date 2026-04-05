using UnityEngine;
using TMPro; 
using UnityEngine.SceneManagement; // This allows the "Restart" logic

public class ScoreManager : MonoBehaviour
{
    // This "Instance" lets other scripts talk to this one easily
    public static ScoreManager instance;

    public TextMeshProUGUI killText; // Someone else connects their UI here
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

    // CALL THIS from your Player script when Health <= 0
    public void TriggerGameOver()
    {
        Debug.Log("Player Died. Restarting Level...");
        // Re-loads the current scene from scratch
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}