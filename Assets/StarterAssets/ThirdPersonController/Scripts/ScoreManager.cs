using UnityEngine;
using TMPro; 
using UnityEngine.SceneManagement; 

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    // --- HUD (While playing) ---
    public TextMeshProUGUI killCounter; 
    public TextMeshProUGUI waveCounter;

    // --- Game Over Screen (When you die) ---
    public TextMeshProUGUI finalKills; 
    public TextMeshProUGUI wavesReached;
    public TextMeshProUGUI highscoreText; 

    public GameObject GameOverScreen; 
    public AudioSource gameOverSource;
    
    private int totalKills = 0;
    private int currentWave = 0;

    void Awake() { instance = this; }

    public void AddKill()
    {
        totalKills++;
        if(killCounter != null) killCounter.text = "Kills: " + totalKills;
    }

    public void UpdateWaveUI(int waveNumber)
    {
        currentWave = waveNumber;
        if(waveCounter != null) waveCounter.text = "Wave: " + waveNumber;
    }

    public void TriggerGameOver()
    {
        if(GameOverScreen != null)
        {
            AudioSource[] allAudioSources = FindObjectsByType<AudioSource>(FindObjectsSortMode.None);
            foreach (AudioSource audio in allAudioSources)
            {
                audio.Stop();
            }

            int bestKills = PlayerPrefs.GetInt("HighScore", 0);
            if (totalKills > bestKills)
            {
                PlayerPrefs.SetInt("HighScore", totalKills);
                PlayerPrefs.Save();
                bestKills = totalKills;
            }

            if (finalKills != null) finalKills.text = "Final Kills: " + totalKills;
            if (wavesReached != null) wavesReached.text = "Waves Reached: " + currentWave;
            if (highscoreText != null) highscoreText.text = "Best Kills: " + bestKills;

            if (gameOverSource != null) gameOverSource.Play();

            GameOverScreen.SetActive(true); 
            Time.timeScale = 0f;           
            Cursor.lockState = CursorLockMode.None; 
            Cursor.visible = true;
        }
    }

    public void LoadMainMenu() { Time.timeScale = 1f; SceneManager.LoadScene("Main Menu"); }
    public void QuitGame() { Application.Quit(); }
}