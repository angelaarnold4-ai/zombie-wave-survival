using UnityEngine;
using TMPro;

public class MainMenuDisplay : MonoBehaviour
{
    public TextMeshProUGUI menuHighDisplay;

    void Start()
    {
        // Get the "HighScore" number we saved in ScoreManager
        int best = PlayerPrefs.GetInt("HighScore", 0);
        
        // Show it on the screen
        if (menuHighDisplay != null)
            menuHighDisplay.text = "High Score: " + best;
    }
}