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
            // --- NEW: Logic for Task #7 (Stop all audio) ---
            // This finds every AudioSource in the game and stops it
            AudioSource[] allAudioSources = FindObjectsByType<AudioSource>(FindObjectsSortMode.None);
            foreach (AudioSource audio in allAudioSources)
            {
                audio.Stop();
            }

            // 1. Handle the High Score (Saving to the computer)
            int bestKills = PlayerPrefs.GetInt("HighScore", 0);
            if (totalKills > bestKills)
            {
                PlayerPrefs.SetInt("HighScore", totalKills);
                PlayerPrefs.Save();
                bestKills = totalKills;
            }

            // 2. Set the text on the Game Over Screen
            if (finalKills != null) finalKills.text = "Final Kills: " + totalKills;
            if (wavesReached != null) wavesReached.text = "Waves Reached: " + currentWave;
            if (highscoreText != null) highscoreText.text = "Best Kills: " + bestKills;

            // 3. Play Game Over sound and show screen
            // Note: We play this AFTER the loop above so it doesn't get stopped!
            if (gameOverSource != null) gameOverSource.Play();

            GameOverScreen.SetActive(true); 
            Time.timeScale = 0f;           
            Cursor.lockState = CursorLockMode.None; 
            Cursor.visible = true;
        }
    }

    public void LoadMainMenu() { Time.timeScale = 1f; SceneManager.LoadScene("MainMenu"); }
    public void QuitGame() { Application.Quit(); }
}