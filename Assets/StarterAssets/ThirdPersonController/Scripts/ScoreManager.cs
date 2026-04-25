using UnityEngine;
using TMPro; 
using UnityEngine.SceneManagement; 

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    public TextMeshProUGUI killText; 
    public TextMeshProUGUI waveText;
    public GameObject GameOverScreen; 
    public AudioSource gameOverSource;
    
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
        if(GameOverScreen != null)
        {
            if (gameOverSource != null)
            {
                gameOverSource.Play();
            }

            GameOverScreen.SetActive(true); 
            Time.timeScale = 0f;           
            Cursor.lockState = CursorLockMode.None; 
            Cursor.visible = true;
        }
    }

    public void LoadMainMenu()
    {
    Time.timeScale = 1f; 
    SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
    Debug.Log("Quit Game Pressed"); 
    Application.Quit(); 
    }
}